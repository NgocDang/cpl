using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Quartz
{
    internal static class QuartzExtensions
    {
        public static void UseQuartz<TIJobFactory>(this IServiceCollection services, params Type[] jobs)
            where TIJobFactory : IJobFactory
        {
            services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton)));

            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;

                scheduler.JobFactory = provider.GetService<TIJobFactory>();

                scheduler.Start();
                return scheduler;
            });
        }

        public static IScheduler GetScheduler<Scheduler, JobFactory>(this IServiceProvider provider)
            where Scheduler : IScheduler
            where JobFactory : IJobFactory
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;

            scheduler.JobFactory = provider.GetService<JobFactory>();
            scheduler.Start();
            return scheduler;
        }
    }
}
