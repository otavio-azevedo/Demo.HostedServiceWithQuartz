using HostedServiceWithQuartz.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HostedServiceWithQuartz.Extensions
{
    public static class QuartzExtensions
    {
        public static void ConfigureQuartz(this IServiceCollection services, IEnumerable<Type> jobs, IConfiguration config)
        {
            //Disable quartz logs
            LogProvider.IsDisabled = true;

            services.AddSingleton<IJobFactory, JobFactory>();
            
            //Intermediate layer between jobs and HostedServices, necessary to allow and handle DI services inside Jobs
            services.AddSingleton<QuartzScopeHandler>();

            services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Scoped)));

            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
                scheduler.JobFactory = provider.GetService<IJobFactory>();

                //Add schedules for jobs and triggers
                foreach (var job in jobs)
                    scheduler.AddJobAndTrigger(job, config);

                return scheduler;
            });

        }

        private static void AddJobAndTrigger(this IScheduler scheduler, Type jobType, IConfiguration config)
        {
            var jobName = jobType.Name;

            var cronSchedule = config[$"Quartz:Schedule:{jobName}"];

            var job = JobBuilder.Create(jobType)
                .WithIdentity(jobName)
                .Build();

            var trigger = TriggerBuilder
                            .Create()
                            .WithIdentity($"{jobName}.trigger")
                            .StartNow();

            if (!string.IsNullOrEmpty(cronSchedule))
                trigger.WithCronSchedule(cronSchedule);

            scheduler.ScheduleJob(job, trigger.Build());
        }
    }
}
