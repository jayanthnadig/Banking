import initstate from "../initstate";
import * as actionTypes from "../../Constants/actionType";
const reportReducer = (state = initstate, action) => {
  switch (action.type) {
    case actionTypes.GET_REPORT_NAMES:
      let _res = action.payload;
      let _reportname = JSON.parse(JSON.stringify(state));
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
        report_details.report_name=[];
        report_details.report_details=[];
        report_details.report_time=new Date().getMilliseconds();
      }
      state = report_details;
      return state;
    case actionTypes.VIEW_REPORT_DETAILS:
      let _viewreport = action.payload;
      let view_details = Object.assign({}, state);
      if (_viewreport.code === 200) {        
        view_details.report_time=new Date().getMilliseconds();
        view_details.report_details = _viewreport.data;
        if(_viewreport.data.length)
          view_details.report_object.reportId=_viewreport.data[0].reportId
      }
      state = view_details;
      return state;
    case actionTypes.EDIT_REPORT_DETAILS:
      let _editreport = action.payload;
      let edit_details = Object.assign({}, state);
      if (_editreport.code === 200) {
        edit_details.report_time=new Date().getMilliseconds();
        edit_details.report_object = _editreport.data;        
      }
      state = edit_details;
      return state;

    case actionTypes.DOWNLOAD_REPORT_DETAILS:
      let _downloadreport = action.payload;
      let download_details = Object.assign({}, state);
      if (_downloadreport.code === 200) {
        //edit_details.report_object=JSON.parse(JSON.stringify(edit_details.report_object));
        // edit_details.report_object = _editreport.data;
      }
      state = download_details;
      return state;
  }
  return state;
};

export default reportReducer;
