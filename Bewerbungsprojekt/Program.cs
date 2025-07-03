using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChronoTask
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(b => b.AddSimpleConsole(o => o.TimestampFormat = "[HH:mm:ss] "))
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IProjectRepository>(
                        new JsonProjectRepository("chronotask.json"));
                    services.AddSingleton<ChronoTaskApp>();
                })
                .Build();

            await host.Services.GetRequiredService<ChronoTaskApp>()
                               .RunAsync();
        }
    }
}
