const Validation = (() => {
  let _loginValidation = (_state) => {
    Object.keys(_state).map((xx) => {
      if (typeof _state[xx] == "object") {
        if (_state[xx].required) {
          if (_state[xx].value.trim() === "") _state[xx].error = true;
          else {
            if (_state[xx].regex) {
              const _emailreg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
              if (!_emailreg.test(_state[xx].value)) {
                _state[xx].error = true;
                _state[xx].errmsg = "Invalid Email format";
              } else _state[xx].error = false;
            } else _state[xx].error = false;
          }
        }
      }
    });
    return _state;
  };

  let _bulkValidation = (_state) => {
    _state.userList.map((xx, ii) => {
      if (xx.userId.trim() == "") _state.userListError[ii][0] = true;
      else _state.userListError[ii][0] = false;
      if (xx.password.trim() == "") _state.userListError[ii][1] = true;
      else _state.userListError[ii][1] = false;
      if (xx.userEmail.trim() == "") _state.userListError[ii][2] = true;
      else {
        const _emailreg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!_emailreg.test(xx.userEmail.trim()))
          _state.userListError[ii][2] = true;
        else _state.userListError[ii][2] = false;
      }
    });
    return _state;
  };
  return {
    LoginValidate: _loginValidation,
    BulkValidation: _bulkValidation,
  };
})();
export default Validation;
