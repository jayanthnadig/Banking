import * as actionTypes from "../../Constants/actionType";
import lookupUtility from "./../../Common/Utility/LookUpDataMapping";
import API from "./../../Constants/API";
import * as requestServices from "../../services/request";

let _profile_data = (_res) => {
  return { type: actionTypes.USER_LOGIN, payload: _res };
};
/*let _getuserprofile_data = (_res) => {
  return { type: actionTypes.GET_USER_DETAILS, payload: _res };
};
let _formprofile_object = (_res) => {
  return { type: actionTypes.PUT_USER_DETAILS, payload: _res };
};
let _insertformprofile_object = (_res) => {
  return { type: actionTypes.POST_USER_DETAILS, payload: _res };
};
let _change_statusobject = () => {
  return { type: actionTypes.CHANGE_STATUS, payload: "new" };
};*/
export const _userLogin = (_obj) => {
    try {
        return (dispatch) => {
          let _res = lookupUtility.LoginObject(_obj);
          requestServices
            .get(API.loginUser, _res)
            .then((res) => {
              console.log("Response", res);
              dispatch(_profile_data(res));
            })
            .catch((err) => {
              console.log("Error", err);
            });
        };
      } catch (e) {
        console.log("actionType-->_post_userdata", e);
      }
};
/*export const _post_userdata = (_obj, _props) => {
  try {
    return (dispatch) => {
      let _res = lookupUtility.UserInsertObject(_obj, _props);
      requestServices
        .post(API.createUser, _res)
        .then((res) => {
          console.log("Response", res);
          dispatch(_insertformprofile_object(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_post_userdata", e);
  }
};
export const _update_userdata = (_obj, _props) => {
  try {
    return (dispatch) => {
      let _res = lookupUtility.UserInsertObject(_obj, _props);
      requestServices
        .put(API.updateUser, _res)
        .then((res) => {
          dispatch(_formprofile_object(res));
        })
        .catch((err) => {
          console.log("Error", err);
        });
    };
  } catch (e) {
    console.log("actionType-->_update_userdata", e);
  }
};*/
