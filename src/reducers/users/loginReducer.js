import initstate from "../initstate";
import * as actionTypes from "../../Constants/actionType";
const loginReducer = (state = initstate, action) => {
  switch (action.type) {
    case actionTypes.USER_LOGIN:
      let _res = action.payload;
      let _login = Object.assign({}, state);
      if (_res.code === 200) {
        _login.loginstatus = true;
        _login.loginmsg = "Success";
        _login.username = _res.data.userId;
        _login.emailid = _res.data.email;
        _login.userid = _res.data.userId;
        _login.token = _res.data.token;
        _login.type = _res.data.type;
        _login.sessionId = _res.data.sessionId;
        _login.expiryTimestamp = _res.data.expiryTimestamp;
        window.localStorage.setItem(
          "userDetails",
          window.btoa(JSON.stringify(_login))
        );
      } else {
        _login.loginstatus = false;
        _login.loginmsg = "Failed";
      }
      state = _login;
      return state;

    case actionTypes.POST_USER_DETAILS:
      let _userDetails = action.payload;
      let _users = Object.assign({}, state);
      if (_userDetails.code === 200) {
        _users.userDetails = JSON.parse(JSON.stringify(_users.userDetails));
        _userDetails.data.map((_data) => {
          if (
            _users.userDetails.filter((xx) => xx.tableId == _data.tableId)
              .length
          ) {
            var _index = -1;
            _users.userDetails.map((yy, jj) => {
              if (yy.tableId == _data.tableId) _index = jj;
            });
            if (_index !== -1) _users.userDetails[_index] = _data;
          } else _users.userDetails.push(_data);
        });
        _users.user_dbStatus=true;
      }
      state = _users;
      return state;
    case actionTypes.GET_USER_DETAILS:
      let _fulluserDetails = action.payload;
      let _emptyusers = Object.assign({}, state);
      if (_fulluserDetails.code === 200) {
        _emptyusers.userDetails = JSON.parse(JSON.stringify(_emptyusers.userDetails));
        _emptyusers.userDetails=_fulluserDetails.data;        
      }
      state = _emptyusers;
      return state;
  }
  return state;
};

export default loginReducer;
