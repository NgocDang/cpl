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

namespace CPL.Misc.Quartz.Jobs
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
            var lotteryService = ((LotteryService)context.JobDetail.JobDataMap["LotteryService"]);
            var sysUserService = ((SysUserService)context.JobDetail.JobDataMap["SysUserService"]);
            var lotteryHistoryService = ((LotteryHistoryService)context.JobDetail.JobDataMap["LotteryHistoryService"]);
            var unitOfWork = ((UnitOfWork)context.JobDetail.JobDataMap["UnitOfWork"]);

            var lotteries = lotteryService.Query()
                    .Include(x => x.LotteryHistories)
                    .Include(x => x.LotteryPrizes)
                    .Select()
                    .Where(x => x.Status.Equals((int)EnumLotteryGameStatus.ACTIVE)
                                && x.Volume.Equals(x.LotteryHistories.Count)
                                && !x.LotteryHistories.Any(y => string.Equals(y.CreatedDate.Date, DateTime.Now.Date)))
                    .Select(x => Mapper.Map<LotteryViewModel>(x));


            var listOfSysUser = sysUserService.Queryable()
                                .Where(x => x.KYCVerified.HasValue);

            if (lotteries != null)
            {
                foreach (var lottery in lotteries)
                {
                    var listUserId = lottery.LotteryHistories.Select(x => x.SysUserId).Distinct();
                    var lstSysUser = listOfSysUser.Where(x => listUserId.Contains(x.Id)).ToList();

                    var listOfWinner = PickWinner(lstSysUser, lottery);
                    var listOfLoser = lottery.LotteryHistories.Where(x => !listOfWinner.Any(w => w.Id == x.Id));

                    var histories = listOfWinner.Concat(listOfLoser).OrderBy(x => x.TicketIndex).ToList();

                    var dataHistories = lotteryHistoryService.Queryable()
                                        .Where(x => x.LotteryId == lottery.Id)
                                        .OrderBy(x=>x.TicketIndex)
                                        .ToList();

                    for (var i=0;i< lottery.Volume; i++)
                    {
                        dataHistories[i].LotteryPrizeId = histories[i].LotteryPrizeId;
                        dataHistories[i].Result = string.IsNullOrEmpty(histories[i].Result) ? EnumGameResult.LOSE.ToString() : histories[i].Result;
                        dataHistories[i].UpdatedDate = DateTime.Now;
                        lotteryHistoryService.Update(dataHistories[i]);
                    }

                    var dataLottery = lotteryService.Queryable()
                                        .Where(x => x.Id == lottery.Id)
                                        .FirstOrDefault();
                    dataLottery.Status = (int)EnumLotteryGameStatus.COMPLETED;
                    dataLottery.UpdatedDate = DateTime.Now;
                    lotteryService.Update(dataLottery);
                }

                unitOfWork.SaveChanges();
            }
        }

        private List<LotteryHistoryViewModel> PickWinner(List<SysUser> listOfSysUser, LotteryViewModel lottery)
        {
            var lastPrize = lottery.LotteryPrizes.LastOrDefault();

            var listOfWinnerTicket = new List<LotteryHistoryViewModel>();

            var lotteryHistoriesGroupedList = lottery.LotteryHistories.GroupBy(x => x.SysUserId).OrderByDescending(g => g.Count())
                                                            .SelectMany(x => x)
                                                            .ToList()
                                                            .Split(lastPrize.Volume);

            for (var i = lottery.LotteryPrizes.Count() - 1; i >= 0; i--)
            {
                var groupPickedIndexs = PickRandomGroup(lotteryHistoriesGroupedList.Count(), lottery.LotteryPrizes[i].Volume).OrderByDescending(x => x);

                foreach (var index in groupPickedIndexs)
                {
                    if (lottery.LotteryPrizes[i].Id == lastPrize.Id)
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
                        lotteryHistoriesGroupedList[index].RemoveAt(winnerIndexInGroup);
                        lotteryHistoriesGroupedList.RemoveAt(index);
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
