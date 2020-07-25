const LookUpUtilities = (() => {
  let _loginObject = (_state) => {
    let _obj = new Object();
    _obj.userId = _state.userId.value;
    _obj.password = _state.password.value;
    return _obj;
  };
  let _loginDetails = () => {
    if (window.localStorage.getItem("userDetails") !== null) {
      var _login = window.atob(window.localStorage.getItem("userDetails"));
      if (_login) _login = JSON.parse(_login);
      else window.location.href = "/";
      return _login;
    }
  };
  let _postDashboard = (_state) => {
    delete _state.modal;
    delete _state.userDetails;
    return _state;
  };
  let _postUserDetails = (_state) => {
    if (!(_state instanceof Array)) {
      let _obj = new Object();
      _obj.userId = _state.userId.value;
      _obj.password = _state.password.value;
      _obj.userEmail = _state.userEmail.value;
      _obj.isAddPermission = _state.isAddPermission.value;
      _obj.isEditPermission = _state.isEditPermission.value;
      _obj.isDeletePermission = _state.isDeletePermission.value;
      _obj.tableId = -1;
      _obj.isActive = true;
      var _finalusers = [];
      _finalusers.push(_obj);
      return _finalusers;
    } else return _state;
  };
  let _setNotificationWindow=(_status,_msg,_flag)=>{
    window["g_dbStatus"]=_status;
    window["g_dbMsg"]=_msg;
    window["g_flag"]=_flag;
  }
  return {
    LoginObject: _loginObject,
    LoginDetails: _loginDetails,
    PostDashboard: _postDashboard,
    UserInsertObject: _postUserDetails,
    SetNotification:_setNotificationWindow
  };
})();
export default LookUpUtilities;
