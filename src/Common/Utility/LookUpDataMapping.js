const LookUpUtilities = (() => {
  let _loginObject = (_state) => {
    let _obj = new Object();
    _obj.userId = _state.userId.value;
      _obj.password = _state.password.value;
    return _obj;
  };
  let _loginDetails=()=>{
      var _login=window.atob(window.localStorage.getItem("userDetails"));
      if(_login)
            _login=JSON.parse(_login)
       else
         window.location.href="/";
      return _login;  
  }
  return {
    LoginObject: _loginObject,
    LoginDetails: _loginDetails
  };
})();
export default LookUpUtilities;
