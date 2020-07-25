using ASNRTech.CoreService.Jobs;
using ASNRTech.CoreService.Logging;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Core
{
    public class SchedulerService
    {
        private static readonly string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        private static IScheduler scheduler;
        private readonly IServiceScope serviceScope;

        public SchedulerService(IServiceProvider container)
        {
            serviceScope = container.CreateScope();
        }

        public async Task StartAsync()
        {
            try
            {
                LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

                await DataSeeder.SeedAsync().ConfigureAwait(false);

                const int MIN_INSECS = 60;
                const int HOUR_INSECS = 60 * 60;

                //NameValueCollection props = new NameValueCollection {
                //  ["quartz.serializer.type"] = "json",
                //  ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                //  ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz",
                //  ["quartz.jobStore.dataSource"] = "default",
                //  ["quartz.dataSource.default.connectionString"] = Utilities.Utility.ConnString,
                //  ["quartz.dataSource.default.provider"] = "Npgsql"
                //};

                //StdSchedulerFactory schedulerFactory = new StdSchedulerFactory(props);

                StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
                scheduler = await schedulerFactory.GetScheduler().ConfigureAwait(false);
                await scheduler.Start().ConfigureAwait(false);

                await ScheduleJobAsync<NotifyErrorsJob>(scheduler, 10 * MIN_INSECS, "NotifyErrors").ConfigureAwait(false);
                await ScheduleJobAsync<SyncUsersJob>(scheduler, HOUR_INSECS, "SyncUsers").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggerService.LogException("", className, "AsyncStart", ex);
            }
        }

        public void Stop()
        {
            // give running jobs 30 sec (for example) to stop gracefully
            if (scheduler?.IsStarted == true)
            {
                if (scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                {
                    scheduler = null;
                }
                else
                {
                    LoggerService.LogInfo(string.Empty, className, "Stop", "Scheduler did not stop in a timely manner.");
                }
            }
        }

        private static async Task ScheduleJobAsync<T>(IScheduler scheduler, int intervalInSeconds, string name) where T : IJob
        {
            JobKey jobKey = new JobKey(name);
            TriggerKey triggerKey = new TriggerKey(name);

            bool jobExists = await scheduler.CheckExists(jobKey).ConfigureAwait(false);
            IJobDetail jobDetail;

            if (jobExists)
            {
                jobDetail = await scheduler.GetJobDetail(jobKey).ConfigureAwait(false);
                ITrigger oldTrigger = await scheduler.GetTrigger(triggerKey).ConfigureAwait(false);

                DateTimeOffset dateTimeOffset = new DateTimeOffset(new DateTime(2030, 1, 1));
                DateTimeOffset? triggerTime1 = oldTrigger.GetFireTimeAfter(dateTimeOffset);
                DateTimeOffset? triggerTime2 = oldTrigger.GetFireTimeAfter(triggerTime1);

                TimeSpan? timeSpan = (triggerTime2 - triggerTime1);

                if (timeSpan.HasValue && timeSpan.Value.TotalSeconds != intervalInSeconds)
                {
                    ITrigger newTrigger = GetSingletonTrigger(intervalInSeconds, name);
                    await scheduler.RescheduleJob(triggerKey, newTrigger).ConfigureAwait(false);
                }
            }
            else
            {
                jobDetail = JobBuilder.Create<T>().WithIdentity(name).Build();
                await scheduler.ScheduleJob(jobDetail, GetSingletonTrigger(intervalInSeconds, name)).ConfigureAwait(false);
            }
        }

        private static ITrigger GetSingletonTrigger(int intervalInSeconds, string name)
        {
            return TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(s => s.WithIntervalInSeconds(intervalInSeconds).WithMisfireHandlingInstructionDoNothing())
                    .WithIdentity(name)
                    .Build();
        }

        private static ITrigger GetTrigger(int intervalInSeconds)
        {
            return TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(s => s.WithIntervalInSeconds(intervalInSeconds))
                    .Build();
        }

        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }
        }
    }
}
