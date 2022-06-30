using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkMetricsSample
{
    static class MetricsReporter
    {
        // for sample purposes only.
        // this should really be reported via StatsD or some other metrics reporting system.
        static ConcurrentDictionary<string, (double total, long count)> _histograms = new();
        static ConcurrentDictionary<string, long> _increments = new();

        public static void Histogram(string name, double value)
        {
            _histograms.AddOrUpdate(name, (value, 1), (name, tuple) => (tuple.total + value, tuple.count + 1));
        }

        public static void Dump()
        {
            foreach (var kvp in _histograms)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value.total / kvp.Value.count:n2} (avg)");
            }
        }
    }
}
