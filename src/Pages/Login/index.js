import React from "react";
import { Redirect } from "react-router-dom";
import { connect } from "react-redux";
import Validation from "../../Common/Utility/Validation";
import * as action_type from "../../actions/users/userManagement"
import logo from "../../dist/images/logo.png";
class Login extends React.Component {
  constructor(props) {
    super(props);
    this.state={
        userId:{type:"userId",value:"",error:false},
        password:{type:"password",value:"",error:false},
        loginstatus:false
    }
  }
  handleChange=(e)=>{
    let _name = e.target.name;
    let _value = e.target.value;
    this.setState(prevState=>{
        let _tempField= Object.assign({},prevState);
        _tempField[_name].value=_value;
        return _tempField;
    })
  }
  Login=()=>{
    let _state=Validation.LoginValidate(this.state);    
    this.setState(prevState=>{
        let _tempField= Object.assign({},prevState);
        _tempField=_state;
        return _tempField;
    });
   let _len= Object.values(_state).filter(xx=>xx.error===true);
   if(!_len.length)
    this.props.USER_LOGIN(_state);
      
      
  }
  componentDidMount(){

  }
  componentWillReceiveProps(nextProps){
        if(nextProps.login_status)
             window.location.href="/dashboard";
             else
             this.setState({
                loginstatus:true
             })     

  }
  render() {
    return (
      <div className="login validate-form">
        <div className="container sm:px-10">
          <div className="block xl:grid grid-cols-2 gap-4">
            <div className="hidden xl:flex flex-col min-h-screen">
              <a href="" className="-intro-x flex items-center pt-5">
                <img
                  alt="Pro Line"   
                  className="w-6"              
                  src={logo}
                />
                <span className="text-white text-lg ml-3">
                  {" "}
               
                </span>
              </a>
              <div className="my-auto">
               
                <div className="-intro-x text-white font-medium text-4xl leading-tight minus-mt-10">
                    Alleviate the monitoring of technical
                  <br />
                  and functional process.
                </div>
                <div className="-intro-x mt-5 text-lg text-white">
                
                </div>
              </div>
            </div>

            <div className="h-screen xl:h-auto flex py-5 xl:py-0 my-10 xl:my-0">
              <div className="my-auto mx-auto xl:ml-20 bg-white xl:bg-transparent px-5 sm:px-8 py-8 xl:p-0 rounded-md shadow-md xl:shadow-none w-full sm:w-3/4 lg:w-2/4 xl:w-auto">
                <h2 className="intro-x font-bold text-2xl xl:text-3xl text-center xl:text-left">
                  Sign In
                </h2>
                <div className="intro-x mt-2 text-gray-500 xl:hidden text-center">
                  A few more clicks to sign in to your account. Manage all your
                  e-commerce accounts in one place
                </div>
                <div className="intro-x mt-8">
                  <input
                    type="text"
                    className={`intro-x login__input input input--lg border border-gray-300 block ${(this.state.userId.error)?"error":""}`}
                    required
                    id="txtemail"
                    placeholder="User Name"
                    name="userId"
                    value={this.state.userId.value}
                    onChange={e => this.handleChange(e)}
                  />
                  <input
                    type="password"
                    className={`intro-x login__input input input--lg border border-gray-300 block mt-4 ${(this.state.password.error)?"error":""}`}
                    required
                    id="txtpass"
                    name="password"
                    placeholder="Password"
                    value={this.state.password.value}
                    onChange={e => this.handleChange(e)}
                  />
                </div>
                {/* <div className="intro-x flex text-gray-700 text-xs sm:text-sm mt-4">
                  <div className="flex items-center mr-auto">
                    <input
                      type="checkbox"
                      className="input border mr-2"
                      id="remember-me"
                    />
                    <label className="cursor-pointer select-none" for="remember-me">
                      Remember me
                    </label>
                  </div>
                  <a href="">Forgot Password?</a>
                </div> */}
                <div className="intro-x mt-5 xl:mt-8 text-center xl:text-left">
                  <button
                    className="button button--lg w-full xl:w-32 text-white bg-theme-1 xl:mr-3"
                    id="btnLogin"
                    onClick={()=>this.Login()}
                  >
                    Login
                  </button>
                  {/* <button className="button button--lg w-full xl:w-32 text-gray-700 border border-gray-300 mt-3 xl:mt-0">
                    Sign up
                  </button> */}
                </div>
                <div className="intro-x mt-10 xl:mt-24 text-gray-700 text-center xl:text-left">
                   <span className={`errorLogin ${(!this.state.loginstatus)?"hide":""}`}>Invalid Credentials</span>
                  <br /> <br /> <br />
                  <a className="text-theme-1" href="">
                    Terms and Conditions
                  </a>{" "}
                  &{" "}
                  <a className="text-theme-1" href="">
                    Privacy Policy
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
const mapProperties = (state) => {
  return {
      login_status:state.loginReducer.loginstatus
   /* profile_lookup: state.createUserReducer.userProfileLook,
    user_details: state.createUserReducer.userData,
    db_status: state.createUserReducer.insertstatus,
    categories: state.allCategories.activeCategories,*/
  };
};
const dispatch_action = (dispatch) => {
  //console.log("userDetails.UserContext.firmId", userDetails.UserContext.firmId)
  
  return {
    USER_LOGIN: (state) => dispatch(action_type._userLogin(state)),
   /* GET_USER_DETAILS: (_id) => dispatch(action_type._get_userdetails(_id)),
    POST_USER_DETAILS: (_state, _props) =>
      dispatch(action_type._post_userdata(_state, _props)),
    PUT_USER_DETAILS: (_state, _props) =>
      dispatch(action_type._update_userdata(_state, _props)),
    CHANGE_STATUS: () => dispatch(action_type._change_status()), */    
  };
};

export default connect(mapProperties, dispatch_action)(Login);
