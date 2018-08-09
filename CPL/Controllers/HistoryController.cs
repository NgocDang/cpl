using System.Collections.Generic;
using System.Linq;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Misc;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CPL.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ILotteryHistoryService _lotteryHistoryService;
        public HistoryController(ILotteryHistoryService lotteryHistoryService)
        {
            this._lotteryHistoryService = lotteryHistoryService;
        }

        public JsonResult SearchLotteryHistory(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchLotteryHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<LotteryHistoryViewModel> SearchLotteryHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var searchBy = (model.search != null) ? model.search.value?.ToLower() : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "desc";
            }

            totalResultsCount = _lotteryHistoryService
                                 .Query()
                                 .Include(x => x.Lottery)
                                 .Include(x => x.LotteryPrize)
                                 .Select()
                                 .Where(x => x.SysUserId == user.Id)
                                 .Count();

            // search the dbase taking into consideration table sorting and paging
            var lotteryHistory = _lotteryHistoryService
                                          .Query()
                                          .Include(x => x.Lottery)
                                          .Include(x => x.LotteryPrize)
                                          .Select()
                                          .Where(x => x.SysUserId == user.Id)
                                          .Select(x => new LotteryHistoryViewModel
                                          {
                                              CreatedDate = x.CreatedDate,
                                              CreatedDateInString = x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss"),
                                              LotteryPhase = x.Lottery.Phase,
                                              LotteryPhaseInString = x.Lottery.Phase.ToString("D3"),
                                              Result = x.Result == EnumGameResult.WIN.ToString() ? "Win" : (x.Result == EnumGameResult.LOSE.ToString() ? "Lose" : (x.Result == EnumGameResult.KYC_PENDING.ToString() ? "KYC Pending" : string.Empty)),
                                              Award = x.LotteryPrizeId.HasValue ? x.LotteryPrize.Value : 0,
                                              AwardInString = x.LotteryPrizeId.HasValue ? x.LotteryPrize.Value.ToString("#,##0.##") : 0.ToString("#,##0.##"),
                                              TicketNumber = !string.IsNullOrEmpty(x.TicketNumber) ? $"{x.Lottery.Phase.ToString("D3")}{CPLConstant.ProjectName}{x.TicketNumber}" : string.Empty,
                                              UpdatedDate = x.UpdatedDate,
                                              UpdatedDateInString = x.UpdatedDate.HasValue ? x.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss") : string.Empty,
                                          });

            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = totalResultsCount;
            }
            else
            {
                lotteryHistory = lotteryHistory
                                        .Where(x => x.CreatedDateInString.ToLower().Contains(searchBy)
                                                    || x.LotteryPhaseInString.ToLower().Contains(searchBy)
                                                    || x.Result.ToLower().Contains(searchBy)
                                                    || x.AwardInString.ToLower().Contains(searchBy)
                                                    || x.TicketNumber.ToLower().Contains(searchBy)
                                                    || x.UpdatedDateInString.ToLower().Contains(searchBy));

                filteredResultsCount = lotteryHistory.Count();
            }

            return lotteryHistory.AsQueryable().OrderBy(sortBy, sortDir).Skip(skip).Take(take).ToList();
        }
    }
}