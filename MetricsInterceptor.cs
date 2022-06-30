using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkMetricsSample
{
    internal class MetricsInterceptor : DbCommandInterceptor
    {
        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            interceptionContext.SetUserState("watch", Stopwatch.StartNew());
        }

        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            var watch = (Stopwatch)interceptionContext.FindUserState("watch");
            watch.Stop();
            MetricsReporter.Histogram("sql.nonquery.duration", watch.ElapsedMilliseconds);
        }

        // todo - also Reader & Scalar...
    }
}
