using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IO;

namespace SentryTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Press ENTER to start...");
            Console.ReadLine();

            Console.WriteLine("Building configuration");
            IConfigurationRoot configuration = buildConfiguration();

            Console.WriteLine("Initialising Sentry");
            var reporter = new SentryErrorReporter(configuration.GetSection("Sentry:Dsn").Value);

            bool loop = true;
            while (loop) { 
                Console.WriteLine("Generating test event. Type test message...");
                string testEventMessage = Console.ReadLine();
                reporter.CaptureAsync(testEventMessage);

                Console.WriteLine("Event sent. Now check Sentry.");
                Console.WriteLine("Press X to exit or Enter to continue");                
                loop = Console.ReadLine().ToLower() != "x";
            }
        }

        private static IConfigurationRoot buildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
