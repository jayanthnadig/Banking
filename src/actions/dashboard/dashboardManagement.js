import * as actionTypes from "../../Constants/actionType";
import lookupUtility from "./../../Common/Utility/LookUpDataMapping";
import API from "./../../Constants/API";
import * as requestServices from "../../services/request";

let _dashboard_ddropdown = (_res) => {
  return { type: actionTypes.GET_ALL_DROPDOWNS, payload: _res };
};
let _dashboard_data = (_res) => {
  return { type: actionTypes.GET_DASHBOARD_WIDGETS, payload: _res };
};
let _new_dashboard_widget = (_res) => {
  return { type: actionTypes.POST_DASHBOARD_WIDGET, payload: _res };
};
let _edit_dashboard_widget = (_res) => {
  return { type: actionTypes.EDIT_DASHBOARD_WIDGET, payload: _res };
};
let _delete_dashboard_widget = (_res) => {
  return { type: actionTypes.DELETE_DASHBOARD_WIDGET, payload: _res };
};
let _drilldown_dashboard_widget = (_res) => {
  return { type: actionTypes.DRILLDOWN_DASHBOARD_WIDGET, payload: _res };
};
let _logout_user = (_res) => {
  return { type: actionTypes.USER_LOGOUT, payload: _res };
};
let _send_mail = (_res) => {
  return { type: actionTypes.SEND_EMAIL, payload: _res };
};
let _set_notification = (_res) => {
  return { type: actionTypes.SET_NOTIFICATION, payload: _res };
};
let _set_spinner = (_res) => {
  return { type: actionTypes.SET_SPINNER };
};
/*let _formprofile_object = (_res) => {
  return { type: actionTypes.PUT_USER_DETAILS, payload: _res };
};
let _insertformprofile_object = (_res) => {
  return { type: actionTypes.POST_USER_DETAILS, payload: _res };
};
let _change_statusobject = () => {
  return { type: actionTypes.CHANGE_STATUS, payload: "new" };
};*/
let _userDetails = lookupUtility.LoginDetails();
export const _getDashboardDropdowns = () => {
  try {    
    return (dispatch) => {
      //let _res = lookupUtility.LoginObject();
      dispatch(_set_spinner());
      requestServices
        .get(API.allWidgetDropDowns, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_dashboard_ddropdown(res));
          dispatch(_set_spinner());
        })
        .catch((err) => {
          console.log("Error", err);
          dispatch(_set_spinner());
        });
    };
  } catch (e) {
    console.log("actionType-->_getDashboardDropdowns", e);
  }
};
export const _getDashboardWidgets = () => {
  try {
    let _userDetails = lookupUtility.LoginDetails();
    return (dispatch) => {
      //let _res = lookupUtility.LoginObject();
      dispatch(_set_spinner());
      requestServices
        .get(API.loadDashboard, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_dashboard_data(res));
          dispatch(_set_spinner());
        })
        .catch((err) => {
          console.log("Error", err);
          dispatch(_set_spinner());
        });
    };
  } catch (e) {
    console.log("actionType-->_getDashboardWidgets", e);
  }
};
export const _post_dashboardWidget = (_obj) => {
  try {
    return (dispatch) => {
      let _res = lookupUtility.PostDashboard(_obj);
      dispatch(_set_spinner());
      requestServices
        .postquery(API.postDashboard, _res, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_new_dashboard_widget(res));        
          dispatch(_set_notification({}));
          dispatch(_set_spinner());
        })
        .catch((err) => {
          console.log("Error", err);
          dispatch(_set_spinner());
        });
    };
  } catch (e) {
    console.log("actionType-->_post_userdata", e);
  }
};

export const _delete_dashboardWidget = (_id) => {
  try {
    return (dispatch) => {
      // let _res = lookupUtility.PostDashboard(_obj);
      dispatch(_set_spinner());
      requestServices
        .deleteQuery(API.deleteDashboard, _userDetails.userid, _id)
        .then((res) => {
          console.log("Response", res);
          res.widgetId = _id;
          dispatch(_delete_dashboard_widget(res));
          dispatch(_set_notification({}));
          dispatch(_set_spinner());
        })
        .catch((err) => {
          console.log("Error", err);
          dispatch(_set_spinner());
        });
    };
  } catch (e) {
    console.log("actionType-->_post_userdata", e);
  }
};

export const _edit_dashboardWidget = (_id) => {
  try {
    return (dispatch) => {
      // let _res = lookupUtility.PostDashboard(_obj);
      dispatch(_set_spinner());
      requestServices
        .getWidgetData(API.editWidget, _userDetails.userid, _id)
        .then((res) => {
          console.log("Response", res);         
          dispatch(_edit_dashboard_widget(res));   
          dispatch(_set_spinner());      
        })
        .catch((err) => {
          console.log("Error", err);
          dispatch(_set_spinner());
        });
    };
  } catch (e) {
    console.log("actionType-->_edit_dashboardWidget", e);
  }
};

export const _post_drilldowndashboardWidget = (_obj) => {
  try {
    return (dispatch) => {
      //let _res = lookupUtility.PostDashboard(_obj);
      dispatch(_set_spinner());
      requestServices
        .postquery(API.drilldownDashboard, _obj, _userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_drilldown_dashboard_widget(res));
          dispatch(_set_spinner());
        })
        .catch((err) => {
          console.log("Error", err);
          dispatch(_set_spinner());
        });
    };
  } catch (e) {
    console.log("actionType-->_post_userdata", e);
  }
};

export const _sendmail_drilldowndashboardWidget = (_obj) => {
  try {
    return (dispatch) => {
      //let _res = lookupUtility.PostDashboard(_obj);
      dispatch(_set_spinner());
      requestServices
        .postquery(API.gridEmailSend, _obj,_userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_send_mail(res));
          dispatch(_set_spinner());
        })
        .catch((err) => {
          console.log("Error", err);
          dispatch(_set_spinner());
        });
    };
  } catch (e) {
    console.log("actionType-->_sendmail_drilldowndashboardWidget", e);
  }
};

export const _user_logout = () => {
  try {
    return (dispatch) => {    
      dispatch(_set_spinner());  
      requestServices
        .postquery(API.logoutUser,"",_userDetails.userid)
        .then((res) => {
          console.log("Response", res);
          dispatch(_logout_user(res));
          dispatch(_set_spinner());
        })
        .catch((err) => {
          console.log("Error", err);
          dispatch(_set_spinner());
        });
    };
  } catch (e) {
    console.log("actionType-->_post_userdata", e);
  }
};

