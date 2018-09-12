using CPL.Common.Enums;
using CPL.Models;
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

namespace CPL.Misc
{
    public interface IAnalyticService
    {
        GetReportsResponse GetPageView(string viewId, DateTime start, DateTime end);
        GetReportsResponse GetBounceRate(string viewId, DateTime start, DateTime end);
        GetReportsResponse GetDevices(string viewId, DateTime start, DateTime end);
    }

    public class AnalyticService : IAnalyticService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _env;
        private readonly AnalyticsReportingService _reportingService;

        public AnalyticService(IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            _env = env;
            _serviceProvider = serviceProvider;


            // Initialize google analytics
            string[] scopes = { AnalyticsReportingService.Scope.AnalyticsReadonly };
            _reportingService = new AnalyticsReportingService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = GoogleCredential.FromJson(CPLConstant.Analytic.Credential).CreateScoped(scopes)
                });
        }

        public GetReportsResponse GetBounceRate(string viewId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public GetReportsResponse GetDevices(string viewId, DateTime start, DateTime end)
        {
            var dateRange = new DateRange
            {
                StartDate = start.ToString("yyyy-MM-dd"),
                EndDate = end.ToString("yyyy-MM-dd")
            };
            var sessions = new Metric
            {
                Expression = "ga:deviceCategory",
                Alias = "Sessions"
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
            return batchRequest.Execute();
        }

        public GetReportsResponse GetPageView(string viewId, DateTime start, DateTime end)
        {
            var dateRange = new DateRange
            {
                StartDate = start.ToString("yyyy-MM-dd"),
                EndDate = end.ToString("yyyy-MM-dd")
            };
            var sessions = new Metric
            {
                Expression = "ga:pageviews",
                Alias = "Sessions"
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
            return batchRequest.Execute();
        }
    }
}
