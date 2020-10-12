import initstate from "../initstate";
import * as actionTypes from "../../Constants/actionType";
const notificationReducer = (state = initstate, action) => {
  switch (action.type) {
    case actionTypes.SET_NOTIFICATION:
      let _notify_status = action.payload;
      let _notify_object = Object.assign({}, state);
      _notify_object.g_dbStatus = window.g_dbStatus;
      _notify_object.g_dbMsg = window.g_dbMsg;
      _notify_object.g_flag = window.g_flag;
      state = _notify_object;
      return state;

    case actionTypes.RESET_NOTIFICATION:
      let _resetnotify_object = Object.assign({}, state);
      _resetnotify_object.g_dbStatus = false;
      state = _resetnotify_object;
      return state;

    case actionTypes.SET_SPINNER:
      let _spinner_object = Object.assign({}, state);
      _spinner_object.loading = !_spinner_object.loading;
      state = _spinner_object;
      return state;
  }
  return state;
};

export default notificationReducer;
