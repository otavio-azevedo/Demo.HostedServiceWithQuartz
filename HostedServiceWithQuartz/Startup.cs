using HostedServiceWithQuartz.Extensions;
using HostedServiceWithQuartz.Jobs;
using HostedServiceWithQuartz.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System.Linq;
using System.Reflection;

namespace HostedServiceWithQuartz
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var teste = Assembly.GetExecutingAssembly();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IFakeScopedService, FakeScopedService>();
                    services.AddHostedService<MyHostedService>();
                    services.ConfigureQuartz(Assembly.GetExecutingAssembly().GetTypes()
                                                     .Where(x => x.FullName.Contains("Jobs")),
                                             context.Configuration);
                });
    }
}
