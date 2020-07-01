import initstate from "../initstate";
import * as actionTypes from "../../Constants/actionType";
const dashboardReducer = (state = initstate, action) => {
    switch(action.type){
        case actionTypes.GET_DASHBOARD_WIDGETS:
            let _res= action.payload;
            let _login=Object.assign({},state);
            if(_res.length){
                _login.loginstatus=true;
                _login.loginmsg="Success";
                _login.username="Ashok";
                _login.emailid="ashok@gmail.com";
                _login.userid=10;
                window.localStorage.setItem("userDetails",window.btoa(JSON.stringify(_login)));
           }else{
            _login.loginstatus=false;
            _login.loginmsg="Failed"
           }
            state=_login;
            return state;
    }
  return state;
};

export default dashboardReducer;
