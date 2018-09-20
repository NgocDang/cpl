using CPL.Common.Enums;
using CPL.Misc.Enums;
using CPL.Models;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Misc
{
    public interface IAnalyticService
    {
        IList<PageViewsViewModel> GetPageViews(string viewId, DateTime start, DateTime end);
        IList<BounceRateViewModel> GetBounceRate(string viewId, DateTime start, DateTime end);
        IList<DeviceCategoryViewModel> GetDeviceCategory(string viewId, DateTime start, DateTime end);
        string CreateView(string viewName);
        string CreateFilter(string filtername, string filterExpression);
        void LinkFilterToView(string viewId, string filterId);
    }

    public class AnalyticService : IAnalyticService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _env;
        private readonly AnalyticsReportingService _reportingService;
        private readonly AnalyticsService _analyticsService;

        public AnalyticService(IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            _env = env;
            _serviceProvider = serviceProvider;


            // Initialize google analytics
            string[] reportingServiceScopes = { AnalyticsReportingService.Scope.AnalyticsReadonly };
            _reportingService = new AnalyticsReportingService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = GoogleCredential.FromJson(CPLConstant.Analytic.Credential).CreateScoped(reportingServiceScopes)
                });

            string[] analyticsServiceScope = { AnalyticsService.Scope.AnalyticsEdit };
            _analyticsService = new AnalyticsService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = GoogleCredential.FromJson(CPLConstant.Analytic.Credential).CreateScoped(analyticsServiceScope)
                });

        }

        public IList<BounceRateViewModel> GetBounceRate(string viewId, DateTime start, DateTime end)
        {
            var dateRange = new DateRange
            {
                StartDate = start.ToString("yyyy-MM-dd"),
                EndDate = end.ToString("yyyy-MM-dd")
            };
            var sessions = new Metric
            {
                Expression = "ga:bounceRate"
            };
            var date = new Dimension { Name = "ga:date" };
            var reportRequest = new ReportRequest
            {
                DateRanges = new List<DateRange> { dateRange },
                Dimensions = new List<Dimension> { date },
                Metrics = new List<Metric> { sessions },
                ViewId = viewId
            };

            var getReportsRequest = new GetReportsRequest
            {
                ReportRequests = new List<ReportRequest> { reportRequest }
            };
            var batchRequest = _reportingService.Reports.BatchGet(getReportsRequest);
            var response = batchRequest.Execute();

            var bounceRates = new List<BounceRateViewModel>();
            foreach (var x in response.Reports.First().Data.Rows)
            {
                if (x.Dimensions.Count > 0 && x.Metrics.Count > 0)
                {
                    bounceRates.Add(new BounceRateViewModel
                    {
                        Date = DateTime.ParseExact(x.Dimensions.First(), "yyyyMMdd", null),
                        Rate = double.Parse(x.Metrics.First().Values.First())
                    });
                }
            }

            return bounceRates;
        }

        public IList<DeviceCategoryViewModel> GetDeviceCategory(string viewId, DateTime start, DateTime end)
        {
            var dateRange = new DateRange
            {
                StartDate = start.ToString("yyyy-MM-dd"),
                EndDate = end.ToString("yyyy-MM-dd")
            };
            var sessions = new Metric
            {
                Expression = "ga:pageviews"
            };
            var deviceCategory = new Dimension { Name = "ga:deviceCategory" };
            var date = new Dimension { Name = "ga:date" };
            var reportRequest = new ReportRequest
            {
                DateRanges = new List<DateRange> { dateRange },
                Dimensions = new List<Dimension> { deviceCategory, date },
                Metrics = new List<Metric> { sessions },
                ViewId = viewId
            };

            var getReportsRequest = new GetReportsRequest
            {
                ReportRequests = new List<ReportRequest> { reportRequest }
            };
            var batchRequest = _reportingService.Reports.BatchGet(getReportsRequest);
            var response = batchRequest.Execute();

            var deviceCategories = new List<DeviceCategoryViewModel>();
            foreach (var x in response.Reports.First().Data.Rows)
            {
                if (x.Dimensions.Count > 0 && x.Metrics.Count > 0)
                {
                    if (string.Compare(x.Dimensions.First(), EnumDeviceCategory.DESKTOP.ToString(), true) == 0)
                        deviceCategories.Add(new DeviceCategoryViewModel
                        {
                            DeviceCategory = (EnumDeviceCategory)Enum.Parse(typeof(EnumDeviceCategory), x.Dimensions[0], true),
                            Date = DateTime.ParseExact(x.Dimensions[1], "yyyyMMdd", null),
                            Count = int.Parse(x.Metrics.First().Values.First())
                        });
                }
            }

            return deviceCategories;
        }

        public IList<PageViewsViewModel> GetPageViews(string viewId, DateTime start, DateTime end)
        {
            var dateRange = new DateRange
            {
                StartDate = start.ToString("yyyy-MM-dd"),
                EndDate = end.ToString("yyyy-MM-dd")
            };
            var sessions = new Metric
            {
                Expression = "ga:pageviews"
            };
            var date = new Dimension { Name = "ga:date" };

            var reportRequest = new ReportRequest
            {
                DateRanges = new List<DateRange> { dateRange },
                Dimensions = new List<Dimension> { date },
                Metrics = new List<Metric> { sessions },
                ViewId = viewId
            };

            var getReportsRequest = new GetReportsRequest
            {
                ReportRequests = new List<ReportRequest> { reportRequest }
            };
            var batchRequest = _reportingService.Reports.BatchGet(getReportsRequest);
            var response = batchRequest.Execute();

            var pageViews = new List<PageViewsViewModel>();
            if (response.Reports.First().Data.Rows != null)
            {
                foreach (var x in response.Reports.First().Data.Rows)
                {
                    if (x.Dimensions.Count > 0 && x.Metrics.Count > 0)
                        pageViews.Add(new PageViewsViewModel
                        {
                            Date = DateTime.ParseExact(x.Dimensions.First(), "yyyyMMdd", null),
                            Count = int.Parse(x.Metrics.First().Values.First())
                        });
                }
            }
            else pageViews.Add(new PageViewsViewModel
            {
                Date = DateTime.Now.Date,
                Count = 0
            });

            return pageViews;
        }

        public string CreateFilter(string filtername, string filterExpression)
        {
            var details = new FilterExpression()
            {
                CaseSensitive = true,
                ExpressionValue = filterExpression,
                Field = "PAGE_REQUEST_URI"
            };

            var body = new Filter()
            {
                Name = filtername,
                Type = "INCLUDE",
                IncludeDetails = details
            };

            var insertRequest = _analyticsService.Management.Filters.Insert(body, Analytic.AccountId);
            var response = insertRequest.Execute();

            return response.Id;
        }

        public string CreateView(string viewName)
        {
            var body = new Profile()
            {
                WebsiteUrl = Analytic.WebsiteUrl,
                Timezone = Analytic.Timezone,
                Name = viewName
            };

            var insertRequest = _analyticsService.Management.Profiles.Insert(body, Analytic.AccountId, Analytic.PropertyId);
            var response = insertRequest.Execute();

            return response.Id;
        }

        public void LinkFilterToView(string viewId, string filterId)
        {
            var filterRef = new FilterRef() {
                Id = filterId
            };

            var body = new ProfileFilterLink() {
                FilterRef = filterRef
            };

            var insertRequest = _analyticsService.Management.ProfileFilterLinks.Insert(body, Analytic.AccountId, Analytic.PropertyId, viewId);
            var response = insertRequest.Execute();
        }
    }
}
