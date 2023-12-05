using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarStore
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build()
            .RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IDoStuff, DoStuff>();
                    services.AddHostedService<Worker>();
                });
        }
    }
}

