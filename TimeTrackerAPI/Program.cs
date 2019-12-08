using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Utilities;

namespace TimeTrackerAPI
{
    public class Program
    {
        private static readonly EventLogger eventlogger = new EventLogger(typeof(Program));

        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            eventlogger.LogInfo("Going to Run()");
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Web Host Builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            eventlogger.LogInfo("Creating Web Host Builder");
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseIIS(
                ).ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    });
        }
    }
}
