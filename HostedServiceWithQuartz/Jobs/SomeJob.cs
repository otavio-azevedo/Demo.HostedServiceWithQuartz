using HostedServiceWithQuartz.Repositories;
using Quartz;
using System;
using System.Threading.Tasks;

namespace HostedServiceWithQuartz.Jobs
{
    public class SomeJob : IJob
    {
        private readonly IFakeScopedService _service;

        public SomeJob(IFakeScopedService service)
        {
            _service = service;
        }
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Running {this.GetType()}... {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
