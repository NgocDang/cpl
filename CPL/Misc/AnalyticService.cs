using CPL.Models;
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
        Task<int> GetPageView(DateTime from, DateTime to);
        Task<double> GetBounceRate(DateTime from, DateTime to);
        Task<List<DeviceViewModel>> GetDevices(DateTime from, DateTime to);
    }

    public class AnalyticService : IAnalyticService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _env;

        public AnalyticService(IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            _env = env;
            _serviceProvider = serviceProvider;
        }

        public Task<double> GetBounceRate(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<List<DeviceViewModel>> GetDevices(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetPageView(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
