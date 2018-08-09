using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace CPL.Controllers
{
    [Permission(EnumRole.Guest)]
    public class NewsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly INewsService _newsService;

        public NewsController(
            IMapper mapper,
            INewsService newsService,
            IUnitOfWorkAsync unitOfWork)
        {
            this._mapper = mapper;
            this._newsService = newsService;
            this._unitOfWork = unitOfWork;
        }


        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = CPLConstant.PageSize;

            var viewModels = _newsService
                              .Queryable()
                              .Select(x => Mapper.Map<NewsViewModel>(x))
                              .OrderByDescending(x => x.Id)
                              .ToPagedList(pageNumber, pageSize);

            return View(viewModels);
        }

        public IActionResult NewsDetail(int id)
        {
            var viewModels = _newsService
                              .Queryable()
                              .Select(x => Mapper.Map<NewsViewModel>(x))
                              .FirstOrDefault(x => x.Id == id);

            return View(viewModels);
        }
    }
}