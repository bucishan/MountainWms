using Pomelo.AspNetCore.TimedJob;
using SqlSugar;
using System;
using M.NetCore.DI;
using M.Utils.Log;

namespace MountainWMS
{
    public class SqlJob : Job
    {
        private readonly SqlSugarClient _client;

        public SqlJob(SqlSugarClient client)
        {
            _client = client;
        }

        [Invoke(IsEnabled = false, Begin = "2018-12-19 10:30", Interval = 1000 * 3, SkipWhileExecuting = true)]
        public void Run()
        {
            var _nlog = ServiceResolve.Resolve<ILogUtil>();
            GC.Collect();
        }
    }
}