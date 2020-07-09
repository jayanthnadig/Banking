import React from "react";
import { connect } from "react-redux";
import Validation from "../../Common/Utility/Validation";
import * as action_type from "../../actions/users/userManagement";
import logo from "../../dist/images/logo.png";

class UserConfig extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
      userId: { type: "userId", value: "", error: false, errmsg: "" },
      password: { type: "password", value: "", error: false, errmsg: "" },
      userEmail: {
        type: "userEmail",
        value: "",
        error: false,
        regex: "email",
        errmsg: "",
      },
      isActive: true,
      tableId: -1,
      userList: [],
      userListError: [],
      userListEdited: [],
      errmsg: "User Already exists",
      duplicate_flag: false,
      db_status: false,
    };
  }
  componentDidMount() {
    this.props.GET_USER_DETAILS("Admin");
  }

  componentWillUnmount() {}

  componentWillReceiveProps(nextProps) {
    console.log(nextProps.userDetails);
    if (nextProps.userDetails.length) {
      this.clear();
      let _errorarray = [];
      let _editedarray = [];
      nextProps.userDetails.map((xx) => {
        _errorarray.push([false, false, false]);
        _editedarray.push(false);
      });
      this.setState({
        userList: nextProps.userDetails,
        userListError: _errorarray,
        userListEdited: _editedarray,
      });
    }
    if (nextProps.db_status) {
      this.setState({
        db_status: true,
      });
      setTimeout(() => {
        this.setState({
          db_status: false,
        });
      }, 2000);
    }
  }
  clear() {
    this.setState({
      userId: { type: "userId", value: "", error: false, errmsg: "" },
      password: { type: "password", value: "", error: false, errmsg: "" },
      userEmail: {
        type: "userEmail",
        value: "",
        error: false,
        errmsg: "",
        regex: "email",
      },
      duplicate_flag: false,
    });
  }
  Focus(e) {
    e.target.select();
  }
  addUserData() {
    var _userobj = new Object();
    _userobj.userId = this.state.userId;
    _userobj.password = this.state.password;
    _userobj.userEmail = this.state.userEmail;
    let _state = Validation.LoginValidate(_userobj);
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField.userId = _state.userId;
      _tempField.password = _state.password;
      _tempField.userEmail = _state.userEmail;
      return _tempField;
    });
    let _len = Object.values(_state).filter((xx) => xx.error === true);
    if (!_len.length) {
      if (
        this.state.userList.filter(
          (xx) =>
            xx.userId.toLowerCase() === this.state.userId.value.toLowerCase()
        ).length
      )
        this.setState({
          duplicate_flag: true,
          errmsg: "User Already exists",
        });
      else {
        this.setState({
          duplicate_flag: false,
          errmsg: "",
        });
        this.props.POST_USER_DETAILS(_state);
      }
    } else {
      this.setState({
        duplicate_flag: true,
        errmsg: _len[0].errmsg ? _len[0].errmsg : "Please fill required field",
      });
    }
  }
  bulkUpdateData() {
    let _finalList = [];
    let _checkDup = new Set(this.state.userList.map((xx) => xx.userId));
    if (_checkDup.size === this.state.userList.length) {
      let _validatedObject = Validation.BulkValidation(
        JSON.parse(JSON.stringify(this.state))
      );

      let bulkerrlist = _validatedObject.userListError.map((xx) => {
        return xx.filter((yy) => yy === true);
      });
      let _bulkerrflag = true;
      bulkerrlist.map((xx) => {
        if (xx.length) _bulkerrflag = false;
      });
      this.setState((prevState) => {
        let _tempField = Object.assign({}, prevState);
        _tempField.userListError = _validatedObject.userListError;
        _tempField.duplicate_flag = !_bulkerrflag;
        _tempField.errmsg =
          _bulkerrflag == false ? "Please fill valid Details" : "";
        return _tempField;
      });
      if (_bulkerrflag) {
        this.state.userListEdited.map((xx, ii) => {
          if (xx) {
            _finalList.push(this.state.userList[ii]);
          }
        });
        if (_finalList.length) this.props.POST_USER_DETAILS(_finalList);
      }
    } else {
      this.setState({
        duplicate_flag: true,
        errmsg: "User Already exists",
      });
    }
  }
  bindUserData() {
    if (this.state.userList.length) {
      return this.state.userList.map((_user, _index) => {
        return (
          <tr>
            <td class="border">{_index + 1}</td>{" "}
            <td class="border">
              <input
                type="text"
                className={`grid_input ${
                  this.state.userListError[_index][0] ? "error" : ""
                }`}
                name="userId"
                value={_user.userId}
                onChange={(e) => this.handleGridChange(e, _index)}
              ></input>
            </td>{" "}
            <td class="border">
              <input
                type="text"
                className={`grid_input ${
                  this.state.userListError[_index][1] ? "error" : ""
                }`}
                name="password"
                value={_user.password}
                onFocus={(e) => this.Focus(e)}
                onChange={(e) => this.handleGridChange(e, _index)}
              ></input>
            </td>{" "}
            <td class="border">
              <input
                type="text"
                className={`grid_input ${
                  this.state.userListError[_index][2] ? "error" : ""
                }`}
                name="userEmail"
                value={_user.userEmail}
                onChange={(e) => this.handleGridChange(e, _index)}
              ></input>
            </td>{" "}
            <td class="border">
              <input
                type="checkbox"
                checked={_user.isActive}
                name="isActive"
                onChange={(e) => this.handleGridChange(e, _index)}
                class="input border mr-2"
                id="vertical-remember-me"
              />
            </td>{" "}
          </tr>
        );
      });
    }
  }
  logout() {
    window.location.href = "/";
  }
  handleChange = (e) => {
    let _name = e.target.name;
    let _value = e.target.value;
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField[_name].value = _value;
      return _tempField;
    });
  };
  handleGridChange = (e, _index) => {
    let _name = e.target.name;
    let _value = "";
    if (e.target.type == "checkbox") _value = e.target.checked;
    else _value = e.target.value;
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField.userList[_index][_name] = _value;
      _tempField.userListEdited[_index] = true;
      return _tempField;
    });
  };
  render() {
    return (
      <>
        <div className="app">
          <div className="border-b border-theme-24 -mt-10 md:-mt-5 -mx-3 sm:-mx-8 px-3 sm:px-8 pt-3 md:pt-0 mb-10">
            <div className="top-bar-boxed flex items-center">
              <a href="" className="-intro-x hidden md:flex">
                <img
                  alt="Midone Tailwind HTML Admin Template"
                  className="w-6"
                  src={logo}
                />
                <span className="text-white text-lg ml-3"> </span>
              </a>

              <div
                class="intro-x dropdown relative mr-4 sm:mr-6"
                style={{ width: "100%" }}
              >
                <span className="custom_logout">Logout</span>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="#000"
                  stroke-width="1.5"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  className="feather feather-log-out mx-auto custom_logout"
                  onClick={() => this.logout()}
                >
                  <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path>
                  <polyline points="16 17 21 12 16 7"></polyline>
                  <line x1="21" y1="12" x2="9" y2="12"></line>
                </svg>
                <span className="userwelcome">Welcome ADMIN</span>
              </div>
            </div>
          </div>
          <div class="flex">
            <nav class="side-nav">
              <ul>
                <li>
                  <a href="/dashboard" class="side-menu ">
                    <div class="side-menu__icon">
                      {" "}
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="1.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        class="feather feather-home"
                      >
                        <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path>
                        <polyline points="9 22 9 12 15 12 15 22"></polyline>
                      </svg>{" "}
                    </div>
                    <div class="side-menu__title"> Dashboard </div>
                  </a>
                </li>

                <li>
                  <a href="/reportconfig" class="side-menu ">
                    <div class="side-menu__icon">
                      {" "}
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="1.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        class="feather feather-inbox"
                      >
                        <polyline points="22 12 16 12 14 15 10 15 8 12 2 12"></polyline>
                        <path d="M5.45 5.11L2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"></path>
                      </svg>{" "}
                    </div>
                    <div class="side-menu__title">
                      {" "}
                      Reports{" "}
                      <i
                        data-feather="chevron-down"
                        class="menu__sub-icon"
                      ></i>{" "}
                    </div>
                  </a>
                </li>

                <li>
                  <a href="#" class="side-menu side-menu--active">
                    <div class="side-menu__icon">
                      {" "}
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="1.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        class="feather feather-users mx-auto"
                      >
                        <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path>
                        <circle cx="9" cy="7" r="4"></circle>
                        <path d="M23 21v-2a4 4 0 0 0-3-3.87"></path>
                        <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
                      </svg>{" "}
                    </div>
                    <div class="side-menu__title"> User Configuration </div>
                  </a>
                </li>
              </ul>
            </nav>

            <div className="content report_content">
              <div class="intro-y flex items-center mt-8">
                <h2 class="text-lg font-medium mr-auto"> </h2>
              </div>
              <div
                className={`intro-y grid grid-cols-12 ${
                  this.state.modal ? "hidden" : ""
                }`}
              >
                <div class="col-span-12 lg:col-span-12">
                  <div class="intro-y box">
                    <div class="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                      <h1 class="font-medium text-base mr-auto text-4xl ">
                        User Configuration
                      </h1>
                    </div>
                    <div class="p-5" id="vertical-bar-chart">
                      <div class="modal__content relative custom_model_content validate-form">
                        <div
                          className={`rounded-md flex items-center px-5 py-4 mb-2 bg-theme-6 txtwhite ${
                            !this.state.duplicate_flag ? "hidden" : ""
                          }`}
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            width="24"
                            height="24"
                            viewBox="0 0 24 24"
                            fill="none"
                            stroke="currentColor"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            class="feather feather-alert-octagon w-6 h-6 mr-2"
                          >
                            <polygon points="7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"></polygon>
                            <line x1="12" y1="8" x2="12" y2="12"></line>
                            <line x1="12" y1="16" x2="12.01" y2="16"></line>
                          </svg>{" "}
                          {this.state.errmsg}{" "}
                        </div>
                        <div
                          className={`rounded-md px-5 py-4 mb-2 bg-theme-9 text-white ${
                            !this.state.db_status ? "hidden" : ""
                          }`}
                        >
                          User added or modified successfully{" "}
                        </div>
                        <div class="flex flex-col sm:flex-row items-center">
                          {" "}
                          <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                            User Name
                          </label>{" "}
                          <input
                            type="text"
                            className={`input w-full border mt-2 flex-1 ${
                              this.state.userId.error ? "error" : ""
                            }`}
                            name="userId"
                            required
                            value={this.state.userId.value}
                            onChange={(e) => this.handleChange(e)}
                            placeholder="User Name"
                          />{" "}
                        </div>{" "}
                        <div class="flex flex-col sm:flex-row items-center">
                          {" "}
                          <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                            Password
                          </label>{" "}
                          <input
                            type="password"
                            className={`input w-full border mt-2 flex-1 ${
                              this.state.password.error ? "error" : ""
                            }`}
                            name="password"
                            required
                            value={this.state.password.value}
                            onChange={(e) => this.handleChange(e)}
                            placeholder=""
                          />{" "}
                        </div>{" "}
                        <div class="flex flex-col sm:flex-row items-center">
                          {" "}
                          <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                            Email
                          </label>{" "}
                          <input
                            type="text"
                            className={`input w-full border mt-2 flex-1 ${
                              this.state.userEmail.error ? "error" : ""
                            }`}
                            placeholder="User Email-Id"
                            required
                            name="userEmail"
                            value={this.state.userEmail.value}
                            onChange={(e) => this.handleChange(e)}
                          />{" "}
                        </div>{" "}
                        <div class="sm:ml-20 sm:pl-5 mt-5">
                          {" "}
                          <button
                            type="button"
                            class="button bg-theme-1 text-white"
                            onClick={() => this.addUserData()}
                          >
                            Create
                          </button>{" "}
                          <button
                            type="button"
                            class="button bg-theme-1 text-white"
                            onClick={() => this.clear()}
                          >
                            Cancel
                          </button>{" "}
                        </div>
                      </div>{" "}
                      <div class="preview">
                        <div class="overflow-x-auto">
                          <h3
                            class="font-medium text-base mr-auto text-2xl "
                            style={{ "margin-bottom": "30px" }}
                          >
                            Users List
                          </h3>{" "}
                          <table class="table">
                            {" "}
                            <thead>
                              {" "}
                              <tr class="text-white">
                                {" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                  SLNO
                                </th>{" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                  User Name
                                </th>{" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                  Password
                                </th>{" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                  Email
                                </th>{" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                  IsActive
                                </th>{" "}
                              </tr>{" "}
                            </thead>{" "}
                            <tbody>{this.bindUserData()}</tbody>{" "}
                          </table>{" "}
                          <div class="sm:ml-20 sm:pl-5 mt-5">
                            {" "}
                            <button
                              type="button"
                              class="button bg-theme-1 text-white"
                              style={{ float: "right" }}
                              onClick={() => this.bulkUpdateData()}
                            >
                              Update
                            </button>{" "}
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </>
    );
  }
}
const mapProperties = (state) => {
  return {
    userDetails: state.loginReducer.userDetails,
    db_status: state.loginReducer.user_dbStatus,
  };
};
const dispatch_action = (dispatch) => {
  return {
    GET_USER_DETAILS: (_id) => dispatch(action_type._get_userdata(_id)),
    POST_USER_DETAILS: (state) => dispatch(action_type._post_userdata(state)),
  };
};

export default connect(mapProperties, dispatch_action)(UserConfig);
