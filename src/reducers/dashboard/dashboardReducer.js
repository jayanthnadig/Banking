import initstate from "../initstate";
import * as actionTypes from "../../Constants/actionType";
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
      if (_newwidget.data.length) {
        _dashboard.widgetData = JSON.parse(
          JSON.stringify(_dashboard.widgetData)
        );
        if (
          _dashboard.widgetData.filter(
            (xx) => xx.widgetId == _newwidget.data[0].widgetId
          ).length
        ) {
          var _index = -1;
          _dashboard.widgetData.map((yy, jj) => {
            if (yy.widgetId == _newwidget.data[0].widgetId) _index = jj;
          });
          if (_index !== -1) _dashboard.widgetData[_index] = _newwidget.data[0];
        } else _dashboard.widgetData.push(_newwidget.data[0]);
      }
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
          if (yy.widgetId == _deletewidget.widgetId) _index = jj;
        });
        if (_index !== -1) _delete_dashboard.widgetData.splice(_index, 1);
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
    case actionTypes.USER_LOGOUT:
      let _logout_status = action.payload;
      let _logout_object = Object.assign({}, state);
      if (_logout_status.code == 200) {        
        _logout_object.loginstatus = false;
        state = _logout_object;
      }
      return state;
  }
  return state;
};

export default dashboardReducer;
