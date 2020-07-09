const LookUpUtilities = (() => {
  let _loginObject = (_state) => {
    let _obj = new Object();
    _obj.userId = _state.userId.value;
    _obj.password = _state.password.value;
    return _obj;
  };
  let _loginDetails = () => {
    var _login = window.atob(window.localStorage.getItem("userDetails"));
    if (_login) _login = JSON.parse(_login);
    else window.location.href = "/";
    return _login;
  };
  let _postDashboard = (_state) => {
    delete _state.modal;
    delete _state.userDetails;
    return _state;
  };
  let _postUserDetails = (_state) => {
    if(!(_state instanceof Array)){
    let _obj = new Object();
    _obj.userId = _state.userId.value;
    _obj.password = _state.password.value;
    _obj.userEmail = _state.userEmail.value;
    _obj.tableId = -1;
    _obj.isActive = true;
    var _finalusers=[];
    _finalusers.push(_obj);
    return _finalusers;
    }
    else
    return _state
  };
  return {
    LoginObject: _loginObject,
    LoginDetails: _loginDetails,
    PostDashboard: _postDashboard,
    UserInsertObject: _postUserDetails,
  };
})();
export default LookUpUtilities;
