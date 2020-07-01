using Quartz;
using System.Threading.Tasks;
using ASNRTech.CoreService.Logging;

namespace ASNRTech.CoreService.Core {
    [DisallowConcurrentExecution]
    public class NotifyErrorsJob : IJob {

        public Task Execute(IJobExecutionContext context) {
            ErrorNotifier.NotifyErrorsAsync(null);

            return null;
        }
    }
}
