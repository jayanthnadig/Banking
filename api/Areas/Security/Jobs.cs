using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Utilities;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Jobs
{
    [DisallowConcurrentExecution]
    internal class SyncUsersJob : IJob
    {
        internal static bool IsRunning = false;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (IsRunning)
                {
                    return;
                }

                IsRunning = true;
                DateTime lastSyncedAt = Utility.SettingGetDate(Constants.SETTING_USERS_LAST_SYNCED_AT);

                Utility.SettingSet(Constants.SETTING_USERS_LAST_SYNCED_AT, DateTime.Now.GetSqlDate());
            }
            catch (Exception ex)
            {
                LoggerService.LogException("", "SyncAnchorsJob", "Execute", ex);
            }
            finally
            {
                IsRunning = false;
            }
        }
    }

    [DisallowConcurrentExecution]
    internal class CalcInvoiceAssociatePayoutsJob : IJob
    {
        internal static bool IsRunning = false;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (IsRunning)
                {
                    return;
                }

                IsRunning = true;

                const int MAX_ITERATIONS = 50;
                int iteration = 0;
                int rowCount = 0;
            }
            catch (Exception ex)
            {
                LoggerService.LogException("", "SyncAnchorsJob", "Execute", ex);
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}
