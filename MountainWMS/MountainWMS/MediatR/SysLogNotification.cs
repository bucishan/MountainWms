using IServices;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using M.Core.Entity;
using M.Utils.Env;

namespace MountainWMS
{
    public class SysLogNotification : INotificationHandler<Sys_log>
    {
        public Task Handle(Sys_log notification, CancellationToken cancellationToken)
        {
            var _log = GlobalCore.GetRequiredService<ISys_logServices>();
            //var _log = ServiceResolve.Resolve<ISys_logServices>();
            _log.Insert(notification);
            return Task.CompletedTask;
        }
    }
}