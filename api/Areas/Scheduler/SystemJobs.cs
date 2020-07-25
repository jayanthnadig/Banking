using ASNRTech.CoreService.Logging;
using Quartz;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Core
{
    [DisallowConcurrentExecution]
    public class NotifyErrorsJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            ErrorNotifier.NotifyErrorsAsync(null);

            return null;
        }
    }
}
