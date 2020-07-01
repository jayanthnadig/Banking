using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.Enums;

namespace TeamLease.CssService.Alcs {
    public class TestSPDataService {
        private static TeamHttpContext httpContext = null;
        private List<SpData> returnValue = new List<SpData>();
        private const int year = 2019;
        private const int month = 2;
        private static readonly DataRequest dataRequest = new DataRequest(year, month, 0, 20);

        public async Task<List<SpData>> Test(HttpContext HttpContext) {
            httpContext = new TeamHttpContext(HttpContext);

            returnValue.Add(await ActionWithDateRangeAsync(AlcsDashboardService.GetActiveAssociateCountAsync).ConfigureAwait(false));
            returnValue.Add(await ActionWithDateRangeAsync(AlcsDashboardService.GetAllAssociateCountAsync).ConfigureAwait(false));
            returnValue.Add(await ActionWithDateRangeAsync(AlcsDashboardService.GetResigneeAssociateCountAsync).ConfigureAwait(false));
            returnValue.Add(await ActionWithDateRangeAsync(AlcsDashboardService.GetNewJoineeAssociateCountAsync).ConfigureAwait(false));
            returnValue.Add(await ActionWithDateRangeAsync(AlcsDashboardService.GetContractExpiryCountAsync).ConfigureAwait(false));

            returnValue.Add(await ActionAsync(AlcsDashboardService.GetPayrollCalendarWidgetAsync).ConfigureAwait(false));
            returnValue.Add(await GetPayoutWidgetAsync().ConfigureAwait(false));
            returnValue.Add(await ActionAsync(AlcsDashboardService.GetContractExpiryDateAsync).ConfigureAwait(false));
            returnValue.Add(await ActionWithDataRequestAsync(AlcsUtilityService.GetBankNamesAsync).ConfigureAwait(false));
            returnValue.Add(await ActionWithDataRequestAsync(AlcsUtilityService.GetPayModesAsync).ConfigureAwait(false));
            return returnValue;
        }

        private async Task<SpData> ActionWithDateRangeAsync<T>(Func<TeamHttpContext, DataRequest, Task<T>> method) {
            string methodName = method.Method.Name;
            try {
                await method(httpContext, dataRequest).ConfigureAwait(false);

                return new SpData(methodName);
            }
            catch (Exception ex) {
                return new SpData(methodName, ex);
            }
        }

        private async Task<SpData> ActionAsync<T>(Func<TeamHttpContext, int, int, Task<T>> method) {
            string methodName = method.Method.Name;
            try {
                await method(httpContext, year, month).ConfigureAwait(false);

                return new SpData(methodName);
            }
            catch (Exception ex) {
                return new SpData(methodName, ex);
            }
        }

        private async Task<SpData> ActionWithDataRequestAsync<T>(Func<TeamHttpContext, DataRequest, Task<T>> method) {
            string methodName = method.Method.Name;
            try {
                await method(httpContext, dataRequest).ConfigureAwait(false);
                return new SpData(methodName);
            }
            catch (Exception ex) {
                return new SpData(methodName, ex);
            }
        }

        private async Task<SpData> ActionWithDataRequestAsync<T>(Func<TeamHttpContext, Task<T>> method) {
            string methodName = method.Method.Name;
            try {
                await method(httpContext).ConfigureAwait(false);
                return new SpData(methodName);
            }
            catch (Exception ex) {
                return new SpData(methodName, ex);
            }
        }

        private async Task<SpData> ActionAsync<T>(Func<TeamHttpContext, Task<T>> method) {
            string methodName = method.Method.Name;
            try {
                await method(httpContext).ConfigureAwait(false);
                return new SpData(methodName);
            }
            catch (Exception ex) {
                return new SpData(methodName, ex);
            }
        }

        private static async Task<SpData> GetPayoutWidgetAsync() {
            try {
                await AlcsDashboardService.GetPayoutWidgetAsync(httpContext, DateTime.Today.Year, DateTime.Today.Month, 7).ConfigureAwait(false);

                return new SpData("GetPayoutWidgetAsync");
            }
            catch (Exception ex) {
                return new SpData("GetPayoutWidgetAsync", ex);
            }
        }
    }
}
