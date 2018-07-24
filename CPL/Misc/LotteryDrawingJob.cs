using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Domain;
using CPL.Infrastructure.Repositories;
using CPL.Misc.Quartz;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public class LotteryDrawingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Drawing(context);
            return Task.FromResult(0);
        }

        public void Drawing(IJobExecutionContext context)
        {
            var listOfSysUser = ((SysUserService)context.JobDetail.JobDataMap["SysUserService"])
                                .Queryable()
                                .Where(x => x.KYCVerified.HasValue).ToList();

            var lotteries = ((LotteryService)context.JobDetail.JobDataMap["LotteryService"])
                    .Query()
                    .Include(x => x.LotteryHistories)
                    .Include(x => x.LotteryPrizes)
                    .Select()
                    .Where(x => x.Status.Equals((int)EnumLotteryGameStatus.ACTIVE)
                                && x.Volume.Equals(x.LotteryHistories.Count)
                                && !x.LotteryHistories.Any(y => string.Equals(y.CreatedDate.Date, DateTime.Now.Date)))
                    .Select(x => Mapper.Map<LotteryViewModel>(x));

            if (!(lotteries is null))
            {
                foreach (var lottery in lotteries)
                {
                    var listOfWinner = PickWinner(listOfSysUser, lottery);
                    var listOfLoser = lottery.LotteryHistories.Where(x => !listOfWinner.Any(w => w.Id == x.Id));

                    var histories = listOfWinner.Concat(listOfLoser).OrderBy(x => x.TicketIndex).ToList();

                    var dataHistories = ((LotteryHistoryService)context.JobDetail.JobDataMap["LotteryHistoryService"])
                                        .Queryable()
                                        .Where(x => x.LotteryId == lottery.Id)
                                        .OrderBy(x=>x.TicketIndex)
                                        .ToList();

                    for (var i=0;i<lottery.Volume;i++)
                    {
                        dataHistories[i].LotteryPrizeId = histories[i].LotteryPrizeId;
                        dataHistories[i].Result = string.IsNullOrEmpty(histories[i].Result) ? EnumGameResult.LOSE.ToString() : histories[i].Result;
                        ((LotteryHistoryService)context.JobDetail.JobDataMap["LotteryHistoryService"]).Update(dataHistories[i]);
                    }
                    try
                    {
                        ((UnitOfWork)context.JobDetail.JobDataMap["UnitOfWork"]).SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private List<LotteryHistoryViewModel> PickWinner(List<SysUser> listOfSysUser, LotteryViewModel lottery)
        {
            var numberOfFourthPrizeWinner = lottery.LotteryPrizes.FirstOrDefault(p => string.Equals(p.Name, CPLConstant.FourthPrize)).Volume;

            var listOfWinnerTicket = new List<LotteryHistoryViewModel>();

            var lotteryHistoriesGroupedList = lottery.LotteryHistories.GroupBy(x => x.SysUserId).OrderByDescending(g => g.Count())
                                                            .SelectMany(x => x)
                                                            .ToList()
                                                            .Split(numberOfFourthPrizeWinner);

            for (var i = lottery.LotteryPrizes.Count() - 1; i >= 0; i--)
            {
                var groupPickedIndexs = PickRandomGroup(lotteryHistoriesGroupedList.Count(), lottery.LotteryPrizes[i].Volume).OrderByDescending(x => x);

                foreach (var index in groupPickedIndexs)
                {
                    if (lottery.LotteryPrizes[i].Name == CPLConstant.FourthPrize)
                    {
                        var winnerIndexInGroup = new Random().Next(0, CPLConstant.LotteryGroupSize);
                        var winner = lotteryHistoriesGroupedList[index][winnerIndexInGroup];
                        winner.LotteryPrizeId = lottery.LotteryPrizes[i].Id;
                        if (IsKYCVerified(listOfSysUser, winner.SysUserId))
                            winner.Result = EnumGameResult.WIN.ToString();
                        else
                            winner.Result = EnumGameResult.KYC_PENDING.ToString();

                        listOfWinnerTicket.Add(winner);
                        lotteryHistoriesGroupedList[index].RemoveAt(winnerIndexInGroup);
                    }
                    else
                    {
                        var winnerIndexInGroup = new Random().Next(0, CPLConstant.LotteryGroupSize - 1);
                        var winner = lotteryHistoriesGroupedList[index][winnerIndexInGroup];
                        winner.LotteryPrizeId = lottery.LotteryPrizes[i].Id;

                        if (IsKYCVerified(listOfSysUser, winner.SysUserId))
                            winner.Result = EnumGameResult.WIN.ToString();
                        else
                            winner.Result = EnumGameResult.KYC_PENDING.ToString();

                        listOfWinnerTicket.Add(winner);
                        lotteryHistoriesGroupedList.RemoveAt(index);
                        lotteryHistoriesGroupedList[index].RemoveAt(winnerIndexInGroup);
                    }
                }
            }

            return listOfWinnerTicket;
        }

        private int[] PickRandomGroup(int maxNumber, int numberOfGroup)
        {
            var rng = new Random();
            return Enumerable.Range(0, maxNumber).OrderBy(x => rng.Next()).Take(numberOfGroup).ToArray();
        }

        private bool IsKYCVerified(List<SysUser> listOfSysUser, int sysUserId)
        {
            return listOfSysUser.Any(x => x.Id == sysUserId);

        }
    }
}
