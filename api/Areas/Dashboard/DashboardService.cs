using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ASNRTech.CoreService.Alcs;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Security;
using ASNRTech.CoreService.Services;

namespace ASNRTech.CoreService.Dashboard {
    public class DashboardService : BaseService {
        //internal static async Task<ResponseBase<AssociateWidget>> GetAssociateWidgetAsync(TeamHttpContext teamHttpContext, int year, int month) {
        //    if (teamHttpContext == null) {
        //        throw new ArgumentNullException(nameof(teamHttpContext));
        //    }

        //    DataRequest dataRequest = new DataRequest(year, month, 0, 0);

        //    int totalCount = await Alcs.DashboardService.GetAllAssociateCountAsync(teamHttpContext, dataRequest).ConfigureAwait(false);
        //    int resigneeCount = await Alcs.DashboardService.GetResigneeAssociateCountAsync(teamHttpContext, dataRequest).ConfigureAwait(false);
        //    int newJoineeCount = await Alcs.DashboardService.GetNewJoineeAssociateCountAsync(teamHttpContext, dataRequest).ConfigureAwait(false);
        //    int contractExpiryCount = await Alcs.DashboardService.GetContractExpiryCountAsync(teamHttpContext, dataRequest).ConfigureAwait(false);
        //    int pendingDojCount = await Alcs.DashboardService.GetPendingDojAssociateCountAsync(teamHttpContext, dataRequest).ConfigureAwait(false);

        //    // proc expects a query string
        //    dataRequest.FilterByQuery = true;
        //    int activeCount = await Alcs.DashboardService.GetActiveAssociateCountAsync(teamHttpContext, dataRequest).ConfigureAwait(false);

        //    AssociateWidget AssociateWidget = new AssociateWidget {
        //        TotalCount = totalCount,
        //        ActiveCount = activeCount,
        //        ResigneeCount = resigneeCount,
        //        NewJoineeCount = newJoineeCount,
        //        ContractExpiryCount = contractExpiryCount,
        //        PendingDojCount = pendingDojCount
        //    };
        //    return GetTypedResponse<AssociateWidget>(teamHttpContext, AssociateWidget);
        //}

        //internal static async Task<ResponseBase<BankingWidget>> GetBankingWidgetAsync(TeamHttpContext teamHttpContext) {
        //    if (teamHttpContext == null) {
        //        throw new ArgumentNullException(nameof(teamHttpContext));
        //    }

        //    BankingWidget bankingWidget = await Alcs.DashboardService.GetBankingWidgetAsync(teamHttpContext).ConfigureAwait(false);

        //    return GetTypedResponse<BankingWidget>(teamHttpContext, bankingWidget);
        //}

        //internal static async Task<ResponseBase<DateTime>> GetContractExpiryDateAsync(TeamHttpContext teamHttpContext) {
        //    DateTime contractExpiry = await Alcs.DashboardService.GetContractExpiryDateAsync(teamHttpContext).ConfigureAwait(false);

        //    return GetTypedResponse<DateTime>(teamHttpContext, contractExpiry);
        //}

        //internal static async Task<PagedResponse<PayoutWidget>> GetPayoutWidgetAsync(TeamHttpContext teamHttpContext) {
        //    if (teamHttpContext == null) {
        //        throw new ArgumentNullException(nameof(teamHttpContext));
        //    }

        //    List<PayoutWidget> returnValue = await Alcs.DashboardService.GetPayoutWidgetAsync(teamHttpContext, DateTime.Today.Year, DateTime.Today.Month, 7).ConfigureAwait(false);

        //    return GetPagingResponse<PayoutWidget>(teamHttpContext, returnValue);
        //}

        //internal static async Task<ResponseBase<PayrollCalendarWidget>> GetPayrollCalendarWidgetAsync(TeamHttpContext teamHttpContext, int year, int month) {
        //    if (teamHttpContext == null) {
        //        throw new ArgumentNullException(nameof(teamHttpContext));
        //    }

        //    PayrollCalendarWidget payrollCalendar = await Alcs.DashboardService.GetPayrollCalendarWidgetAsync(teamHttpContext, year, month).ConfigureAwait(false);

        //    return GetTypedResponse<PayrollCalendarWidget>(teamHttpContext, payrollCalendar);
        //}

        internal static async Task<ResponseBase<List<LoadWidgets>>> GetWidgetAsync(TeamHttpContext teamHttpContext, string widgettype, string widgetname, string widgetquery) {
            if (teamHttpContext == null) {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            //DataRequest dataRequest = new DataRequest(year, month, 0, 0);
            List<LoadWidgets> Widgets = await Alcs.DashboardService.GetWidgetAsync(teamHttpContext, widgettype, widgetname, widgetquery).ConfigureAwait(false);
            return GetTypedResponse(teamHttpContext, Widgets);
        }

        internal static async Task<ResponseBase<List<LoadWidgets>>> GetAllWidgetAsync(TeamHttpContext teamHttpContext) {
            if (teamHttpContext == null) {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            //DataRequest dataRequest = new DataRequest(year, month, 0, 0);
            List<LoadWidgets> Widgets = await Alcs.DashboardService.GetAllWidgetAsync(teamHttpContext).ConfigureAwait(false);
            return GetTypedResponse(teamHttpContext, Widgets);
        }

        internal static async Task<ResponseBase<List<LoadWidgets>>> PostWidgetAsync(TeamHttpContext teamHttpContext, DashboardWidget details) {
            if (teamHttpContext == null) {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            if (details.WidgetID < 0) {
                //ResponseBase response = await Alcs.DashboardService.createWidget(teamHttpContext, details).ConfigureAwait(false);
                //if (response.Code == HttpStatusCode.OK) {
                //    List<LoadWidgets> Widgets = await Alcs.DashboardService.GetWidgetAsync(teamHttpContext, details.WidgetType, details.WidgetName, details.WidgetQuery).ConfigureAwait(false);
                //    return GetTypedResponse(teamHttpContext, Widgets);
                //}
            }
            return null;
        }

        //internal static ResponseBase<List<Transactions>> GetTransactions(TeamHttpContext teamHttpContext) {
        //    if (teamHttpContext == null) {
        //        throw new ArgumentNullException(nameof(teamHttpContext));
        //    }
        //    //DataRequest dataRequest = new DataRequest(year, month, 0, 0);
        //    List<Transactions> objtran = Alcs.DashboardService.GetTransactions(teamHttpContext);
        //    return GetTypedResponse(teamHttpContext, objtran);
        //}
    }
}
