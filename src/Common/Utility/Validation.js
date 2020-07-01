const Validation = (() => {
    let _loginValidation = (_state) => {       
      Object.keys(_state).map(xx=>{
          if(typeof _state[xx]=="object"){
            if(_state[xx].value.trim()==="")
                _state[xx].error=true;
            else   
                _state[xx].error=false;
          }
      });
      return _state;
    };
    return {
      LoginValidate: _loginValidation,
    };
  })();
  export default Validation;
  