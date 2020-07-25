import * as actionTypes from "../../Constants/actionType";

let _notify_data = (_res) => {
    return { type: actionTypes.SET_NOTIFICATION, payload: _res };
  };
  let _resetnotify_data = (_res) => {
    return { type: actionTypes.RESET_NOTIFICATION, payload: _res };
  };


  export const _setNotify = (_obj) => {
    try {
        return (dispatch) => {
              dispatch(_notify_data(_obj));          
        }  
      } catch (e) {
        console.log("actionType-->_setNotify", e);
      }
}; 

export const _resetNotify = (_obj) => {
    try {
        return (dispatch) => {
              dispatch(_resetnotify_data(_obj));          
        }  
      } catch (e) {
        console.log("actionType-->_resetNotify", e);
      }
}; 