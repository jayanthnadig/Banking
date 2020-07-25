using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace ASNRTech.CoreService.Core
{
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScope scope;

        public JobFactory(IServiceScope scope)
        {
            this.scope = scope;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
    }
}
