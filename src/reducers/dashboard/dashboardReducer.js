import initstate from "../initstate";
import * as actionTypes from "../../Constants/actionType";
import LookUpUtilities from "../../Common/Utility/LookUpDataMapping";
const dashboardReducer = (state = initstate, action) => {
  switch (action.type) {
    case actionTypes.GET_DASHBOARD_WIDGETS:
      let _res = action.payload;
      let _login = Object.assign({}, state);
      if (_res.data.length) {
        _login.widgetData = _res.data;
      }
      state = _login;
      return state;
    case actionTypes.POST_DASHBOARD_WIDGET:
      let _newwidget = action.payload;
      let _dashboard = Object.assign({}, state);
      var _index = -1;
      if (_newwidget.data.length) {
        _dashboard.widgetData = JSON.parse(
          JSON.stringify(_dashboard.widgetData)
        );
        if (
          _dashboard.widgetData.filter(
            (xx) => xx.dashboardWidgetId == _newwidget.data[0].widgetId
          ).length
        ) {
          _dashboard.widgetData.map((yy, jj) => {
            if (yy.dashboardWidgetId == _newwidget.data[0].widgetId)
              _index = jj;
          });
          if (_index !== -1) {
            _dashboard.widgetData[_index].dashbaordWidgetData =
              _newwidget.data[0].widgetData;
            _dashboard.widgetData[_index].dashboardWidgetName =
              _newwidget.data[0].dashboardWidgetName;
            _dashboard.widgetData[_index].dashboardWidgetType =
              _newwidget.data[0].dashboardChartType;
          }
        } else {
          var _obj = new Object();
          _obj.dashboardWidgetId = _newwidget.data[0].widgetId;
          _obj.dashbaordWidgetData = _newwidget.data[0].widgetData;
          _obj.dashboardWidgetName = _newwidget.data[0].dashboardWidgetName;
          _obj.dashboardWidgetType = _newwidget.data[0].dashboardChartType;
          _dashboard.widgetData.push(_obj);
        }
        _dashboard.singleDashboardWidget = {};
      }

      let _msg =
        _index === -1
          ? "Widget Added Successfully"
          : "Widget Updated Successfully";
      LookUpUtilities.SetNotification(true, _msg, 1);
      state = _dashboard;
      return state;

    case actionTypes.DELETE_DASHBOARD_WIDGET:
      let _deletewidget = action.payload;
      let _delete_dashboard = Object.assign({}, state);
      if (_deletewidget.code == 200) {
        _delete_dashboard.widgetData = JSON.parse(
          JSON.stringify(_delete_dashboard.widgetData)
        );
        var _index = -1;
        _delete_dashboard.widgetData.map((yy, jj) => {
          if (yy.dashboardWidgetId == _deletewidget.widgetId) _index = jj;
        });
        if (_index !== -1) _delete_dashboard.widgetData.splice(_index, 1);

        LookUpUtilities.SetNotification(true, "Widget Deleted Successfully", 1);
        state = _delete_dashboard;
      }
      return state;

    case actionTypes.DRILLDOWN_DASHBOARD_WIDGET:
      let _drilldowndata = action.payload;
      let _drilldown_dashboard = Object.assign({}, state);
      if (_drilldowndata.code == 200) {
        _drilldown_dashboard.drilldownData = JSON.parse(
          JSON.stringify(_drilldown_dashboard.drilldownData)
        );
        _drilldown_dashboard.drilldownData = _drilldowndata.data;
        state = _drilldown_dashboard;
      }
      return state;

    case actionTypes.EDIT_DASHBOARD_WIDGET:
      let _editdashboardwidget = action.payload;
      let _db_editdashboardwidget = Object.assign({}, state);
      if (_editdashboardwidget.code == 200) {
        _db_editdashboardwidget.singleDashboardWidget =
          _editdashboardwidget.data;
        state = _db_editdashboardwidget;
      }
      return state;
    case actionTypes.GET_ALL_DROPDOWNS:
      let _dashboarddropdown = action.payload;
      let _db_dashboarddropdown = Object.assign({}, state);
      if (_dashboarddropdown.code == 200) {
        _db_dashboarddropdown.dashboardDropdown = _dashboarddropdown.data;
        state = _db_dashboarddropdown;
      }
      return state;
    case actionTypes.USER_LOGOUT:
      let _logout_status = action.payload;
      let _logout_object = Object.assign({}, state);
      if (_logout_status.code == 200) {
        _logout_object.loginstatus = false;
        state = _logout_object;
      }
      return state;

    /*case actionTypes.SET_NOTIFICATION:
      let _notify_status = action.payload;
      let _notify_object = Object.assign({}, state);
      if (!Object.keys(_notify_status).length) {
        _notify_object.g_dbStatus = false;
      }
      state = _notify_object;
      return state;*/

    case actionTypes.SEND_EMAIL:
      return state;
  }
  return state;
};

export default dashboardReducer;
