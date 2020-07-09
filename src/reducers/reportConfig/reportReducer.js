import initstate from "../initstate";
import * as actionTypes from "../../Constants/actionType";
const reportReducer = (state = initstate, action) => {
  switch (action.type) {
    case actionTypes.GET_REPORT_NAMES:
      let _res = action.payload;
      let _reportname = Object.assign({}, state);
      if (_res.data.length) {
        _reportname.report_name = _res.data;
      }
      state = _reportname;
      return state;
    case actionTypes.POST_REPORT_DETAILS:
      let _newreport = action.payload;
      let report_details = Object.assign({}, state);
      if (_newreport.code === 200) {
        report_details.report_dbStatus = true;
      }
      state = report_details;
      return state;
    case actionTypes.VIEW_REPORT_DETAILS:
      let _viewreport = action.payload;
      let view_details = Object.assign({}, state);
      if (_viewreport.code === 200) {
        view_details.report_details = _viewreport.data;
      }
      state = view_details;
      return state;
    case actionTypes.EDIT_REPORT_DETAILS:
      let _editreport = action.payload;
      let edit_details = Object.assign({}, state);
      if (_editreport.code === 200) {
        edit_details.report_object=JSON.parse(JSON.stringify(edit_details.report_object));
        edit_details.report_object = _editreport.data;
      }
      state = edit_details;
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
  }
  return state;
};

export default reportReducer;
