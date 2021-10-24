using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;

namespace HostedServiceWithQuartz.Base
{
    public class QuartzScopeHandler : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        public QuartzScopeHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobType = context.JobDetail.JobType;
                var job = scope.ServiceProvider.GetRequiredService(jobType) as IJob;

                await job.Execute(context);
            }
        }
    }

}
