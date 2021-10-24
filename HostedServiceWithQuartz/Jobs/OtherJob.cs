using Quartz;
using System;
using System.Threading.Tasks;

namespace HostedServiceWithQuartz.Jobs
{
    public class OtherJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Running {this.GetType()}... {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
