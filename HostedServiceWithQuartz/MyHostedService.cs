using HostedServiceWithQuartz.Extensions;
using HostedServiceWithQuartz.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceWithQuartz
{
    public class MyHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;

        public MyHostedService(IConfiguration configuration, IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Start(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
