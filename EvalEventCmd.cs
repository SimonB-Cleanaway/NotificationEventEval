using Serilog;
using System.Collections.Generic;
using System.Linq;
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

            // Load Rules into a dictionary of based on rule type.
            var rules = LoaderFactory.LoadNotificationRulesFromFile(args.First())
                .GroupBy(x => x.NotificationType)
                .ToDictionary(x => x.Key, x => x.ToArray());

            // Get list of rules and the matching evaluators based on rule type and evaluator supported types
            var ruleEvals = _notificationEvals
                .SelectMany(x => x.SupportNotificationTypes.Select(nt => (eval: x, notificationType: nt)))
                .Select(x => rules.TryGetValue(x.notificationType, out var supportedRules) ? (x.eval, supportedRules) : (x.eval, null))
                .Where(x => x.supportedRules != null)
                .SelectMany(x => x.supportedRules.Select(y => (rule: y, x.eval)))
                .ToList();

            // Go through all the stream events
            foreach (var sef in args.Skip(1))
            {
                var streamEvent = LoaderFactory.LoadStreamEventFromFile(sef);

                var tasks = ruleEvals.Select(x => x.eval.Evaluate(x.rule, streamEvent));
                var results = await Task.WhenAll(tasks);

                foreach(var result in results.Where(x => x != null))
                {
                    _logger.Information($"{result.Level} - {result.Message}");
                }
            }
        }
    }
}
