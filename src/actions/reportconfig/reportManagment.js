import * as actionTypes from "../../Constants/actionType";
import lookupUtility from "./../../Common/Utility/LookUpDataMapping";
import API from "./../../Constants/API";
import * as requestServices from "../../services/request";

let _report_names = (_res) => {
  return { type: actionTypes.GET_REPORT_NAMES, payload: _res };
};
let _new_report_details = (_res) => {
  return { type: actionTypes.POST_REPORT_DETAILS, payload: _res };
};
let _view_report_details = (_res) => {
  return { type: actionTypes.VIEW_REPORT_DETAILS, payload: _res };
};
let _edit_report_details = (_res) => {
  return { type: actionTypes.EDIT_REPORT_DETAILS, payload: _res };
};
let _download_report_details = (_res) => {
  return { type: actionTypes.DOWNLOAD_REPORT_DETAILS, payload: _res };
};
let _delete_dashboard_widget = (_res) => {
  return { type: actionTypes.DELETE_DASHBOARD_WIDGET, payload: _res };
};
let _drilldown_dashboard_widget = (_res) => {
  return { type: actionTypes.DRILLDOWN_DASHBOARD_WIDGET, payload: _res };
};
let _get_schedulers_names = (_res) => {
  return { type: actionTypes.GET_SCHEDULER_NAMES, payload: _res };
};
let _new_schedulers_names = (_res) => {
  return { type: actionTypes.POST_SCHEDULER_NAME, payload: _res };
};
let _edit_schedulers_names = (_res) => {
  return { type: actionTypes.EDIT_SCHEDULER_NAME, payload: _res };
};
let _set_notification = (_res) => {
  return { type: actionTypes.SET_NOTIFICATION, payload: _res };
};

let _userDetails = lookupUtility.LoginDetails();
export const _getReportNames = () => {
  try {
    return (dispatch) => {
      //let _res = lookupUtility.LoginObject();
      requestServices
        .get(API.getReportName, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_report_names(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_getReportNames", e);
  }
};
export const _post_ReportDetails = (_obj) => {
  try {
    return (dispatch) => {
      // let _res = lookupUtility.PostDashboard(_obj);
      requestServices
        .postquery(API.postReportName, _obj, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_new_report_details(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_post_ReportDetails", e);
  }
};

export const _view_ReportDetails = (_obj) => {
  try {
    return (dispatch) => {
      // let _res = lookupUtility.PostDashboard(_obj);
      requestServices
        .getQuery(API.viewReportName, _userDetails.userid, _obj)
        .then((res) => {
          console.log("Response", res);
          dispatch(_view_report_details(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_view_ReportDetails", e);
  }
};

export const _edit_ReportDetails = (_obj) => {
  try {
    return (dispatch) => {
      // let _res = lookupUtility.PostDashboard(_obj);
      requestServices
        .getQuery(API.editReportName, _userDetails.userid, _obj)
        .then((res) => {
          console.log("Response", res);
          dispatch(_edit_report_details(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_edit_ReportDetails", e);
  }
};

export const _delete_dashboardWidget = (_id) => {
  try {
    return (dispatch) => {
      // let _res = lookupUtility.PostDashboard(_obj);
      requestServices
        .deleteQuery(API.deleteDashboard, _userDetails.userid, _id)
        .then((res) => {
          console.log("Response", res);
          res.widgetId = _id;
          dispatch(_delete_dashboard_widget(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_delete_dashboardWidget", e);
  }
};

export const _post_drilldowndashboardWidget = (_obj) => {
  try {
    return (dispatch) => {
      //let _res = lookupUtility.PostDashboard(_obj);
      requestServices
        .postquery(API.drilldownDashboard, _obj, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_drilldown_dashboard_widget(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_post_drilldowndashboardWidget", e);
  }
};
export const _get_scheduler_names = () => {
  try {
    return (dispatch) => {
      //let _res = lookupUtility.PostDashboard(_obj);
      requestServices
        .get(API.getSchedulerNames, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_get_schedulers_names(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_post_drilldowndashboardWidget", e);
  }
};

export const _post_SchedulerName = (_obj) => {
  try {
    return (dispatch) => {
      // let _res = lookupUtility.PostDashboard(_obj);
      requestServices
        .postquery(API.addUpdateSchedular, _obj, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_new_schedulers_names(res));
          dispatch(_set_notification({}));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_post_ReportDetails", e);
  }
};

export const _edit_SchedulerName = (_id) => {
  try {
    return (dispatch) => {
      // let _res = lookupUtility.PostDashboard(_obj);
      requestServices
        .getScheduler(API.editSchedulerNames, _userDetails.userid, _id)
        .then((res) => {
          console.log("Response", res);
          dispatch(_edit_schedulers_names(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_edit_SchedulerName", e);
  }
};

export const _clear_Scheduler = (_id) => {
  try {
    return (dispatch) => {
      dispatch({ type: actionTypes.CLEAR_SCHEDULER, payload: {} });
    };
  } catch (e) {
    console.log("actionType-->_clear_Scheduler", e);
  }
};
