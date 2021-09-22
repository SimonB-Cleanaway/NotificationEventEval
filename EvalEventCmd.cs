using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    public class EvalEventCmd : ICmd
    {
        private readonly INotificationEvaluator[] _notificationEvals;
        private readonly ILogger _logger;

        public EvalEventCmd(
            IEnumerable<INotificationEvaluator> notificationsEvals,
            ILogger logger)
        {
            _notificationEvals = notificationsEvals.ToArray();
            _logger = logger;
        }

        public async Task Run(IEnumerable<string> args)
        {
            if (!args.Any())
            {
                _logger.Information("Usage: <Rules.Json> [StreamEvents, ....]");
                return;
            }

            var rules = LoaderFactory.LoadNotificationRulesFromFile(args.First())
                .GroupBy(x => x.NotificationType)
                .ToDictionary(x => x.Key, x => x.ToArray());

            foreach (var sef in args.Skip(1))
            {
                var streamEvent = LoaderFactory.LoadStreamEventFromFile(sef);

                foreach(var (eval, et) in _notificationEvals.SelectMany(x => x.SupportNotificationTypes.Select(t => (x, t))))
                {
                    if (!rules.TryGetValue(et, out var supportedRules))
                        continue;

                    foreach (var rule in supportedRules)
                    {
                        if (await eval.Evaluate(rule, streamEvent))
                        {
                            _logger.Information("Yeah");
                        }
                    }

                }
            }
        }
    }
}
