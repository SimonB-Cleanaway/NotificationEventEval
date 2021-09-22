using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace SampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            logger.Information("Starting...");

            var serviceProvider = ConfigureIoC(logger);

            var cmds = typeof(Program).Assembly.GetTypes()
               .Where(x => x.IsClass && typeof(ICmd).IsAssignableFrom(x))
               .ToDictionary(x => x.Name.EndsWith("Cmd", StringComparison.OrdinalIgnoreCase) ? x.Name[0..^3] : x.Name, StringComparer.OrdinalIgnoreCase);

            if (args.Length < 1)
            {
                logger.Error("No command specified.");
                logger.Information("Commands are: " + string.Join(",", cmds.Keys));
                return;
            }
            
            if (cmds.TryGetValue(args[0], out var cmdType))
            {
                var cmd = (ICmd)serviceProvider.GetService(cmdType);
                await cmd.Run(args.Skip(1));
            }
            else
            {
                logger.Error("Unknown command :" + args[0]);
                return;
            }
        }

        private static ServiceProvider ConfigureIoC(ILogger logger)
        {
            var sc = new ServiceCollection()
                .AddSingleton(logger);

            foreach(var cmdType in typeof(Program).Assembly.GetTypes()
                .Where(x => x.IsClass && typeof(ICmd).IsAssignableFrom(x)))
            {
                sc.AddTransient(cmdType);
            }

            foreach (var notificationType in typeof(Program).Assembly.GetTypes()
                .Where(x => x.IsClass && typeof(INotificationEvaluator).IsAssignableFrom(x)))
            {
                sc.AddTransient(typeof(INotificationEvaluator), notificationType);
            }

            return sc.BuildServiceProvider();
        }
    }
}
