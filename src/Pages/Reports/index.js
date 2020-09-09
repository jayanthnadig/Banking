import React from "react";
import { connect } from "react-redux";
import LookUpUtilities from "../../Common/Utility/LookUpDataMapping";
import * as action_type from "../../actions/dashboard/dashboardManagement";
import logo from "../../dist/images/logo.png";

class Report extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
      header: "",
      level: 0,
      send_mail_pop: false,
      reportDeliveryMode: "email",
      startIndex: 0,
      endIndex: 11,
      pagination: false,
      gridlength: 0,
      emailOption: "1",
      emailReceipient: "",
      fileFormat: "",
      phoneNo: "",
      defaultSMS: "",
      gridColumnName: "",
      emailReceipientError:false
    };
    this.mailobj = new Object();
  }
  componentDidMount() {
    let _params = window.location.pathname.split("/");
    let _obj = new Object();
    _obj.clickLevel = "L" + _params[_params.length - 4];
    _obj.clickedWidgetId = parseInt(_params[_params.length - 3]);
    _obj.clickedOnValue = _params[_params.length - 2];
    _obj.gridColumns = [];
    _obj.gridInput =
      _params[_params.length - 1] === "null"
        ? []
        : JSON.parse(window.atob(_params[_params.length - 1]));
    _obj.gridData = [];
    this.setState({
      header: _obj.clickedOnValue,
      level: _params[_params.length - 4],
    });
    this.mailobj = _obj;
    this.props.GET_DASHBOARD_DRILLDOWN(_obj);
  }

  componentWillUnmount() {}
  componentWillReceiveProps(nextProps) {
    console.log(nextProps);
    if (nextProps.drilldown_data.length) {
      this.props.SEND_DASHBOARD_MAIL(this.mailobj);
      if (nextProps.drilldown_data[0].gridData.length > 20)
        this.setState({
          pagination: true,
          gridlength: nextProps.drilldown_data[0].gridData.length,
        });
    }
  }
  buildDropdownOptions() {
    if (this.props.drilldown_data.length) {
      return this.props.drilldown_data[0].gridColumns.map((_head, _index) => {
        return <option value={_head}>{_head}</option>;
      });
    }
  }
  handleChange = (e, _op) => {
    const _emailreg = /^(\s?[^\s,]+@[^\s,]+\.[^\s,]+\s?,)*(\s?[^\s,]+@[^\s,]+\.[^\s,]+)$/;
    let _name = e.target.name;
    let _value = "";
    if (e.target.type == "checkbox" || e.target.type == "radio")
      _value = e.target.checked;      
    else _value = e.target.value;
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      if (_name === "reportDeliveryMode") {
        _tempField[_name] = _tempField[_name] === "email" ? "sms" : "email";
      }else if (_name.includes("Dummy")) {
        _tempField[_name] = _value;
        if (_value.indexOf(";") !== -1) {
          if (_emailreg.test(_value.trim())) {
            _tempField[_name.split("Dummy")[0]] = _tempField[
              _name.split("Dummy")[0]
            ].length
              ? _tempField[_name.split("Dummy")[0]] + ";" + _value.trim()
              : _value.trim();
            _tempField[_name] = "";
          } else {
            LookUpUtilities.SetNotification(true, "Invalid EmailId", 2);
            this.props.SET_NOTIFICATION();
          }
        }
      } else _tempField[_name] = _value;
      return _tempField;
    });
  };

  bindEnteredEmail(e, _obj) {
    e.stopPropagation();
    e.preventDefault();
    const _emailreg = /^(\s?[^\s,]+@[^\s,]+\.[^\s,]+\s?,)*(\s?[^\s,]+@[^\s,]+\.[^\s,]+)$/;
    if (this.state[_obj].trim().length) {
      if (_emailreg.test(this.state[_obj].trim())) {
        var _combemail = this.state[_obj.split("Dummy")[0]].length
          ? this.state[_obj.split("Dummy")[0]] + ";" + this.state[_obj].trim()
          : this.state[_obj].trim();
        this.setState((prevState) => {
          let _tempField = Object.assign({}, prevState);
          _tempField[_obj] = "";
          _tempField[_obj.split("Dummy")[0]] = _combemail;
          return _tempField;
        });
      } else {
        this.setState((prevState) => {
          let _tempField = Object.assign({}, prevState);
          _tempField[_obj] = "";
          return _tempField;
        });
      }
    }
    return false;
  }
  drillDown(e, _data) {
    var _list = [];
    document
      .querySelectorAll("#vertical-bar-chart table tr")
      .forEach((xx) => xx.removeAttribute("class"));
    e.currentTarget.setAttribute("class", "td_highlight");
    this.props.drilldown_data[0].gridColumns.map((xx, ii) => {
      var _obj = new Object();
      _obj.name = xx;
      _obj.value = _data[ii];
      _list.push(_obj);
    });

    let _params = window.location.pathname.split("/");
    let _obj = new Object();
    _obj.clickLevel = parseInt(_params[_params.length - 4]) + 1;
    _obj.clickedWidgetId = parseInt(_params[_params.length - 3]);
    _obj.clickedOnValue = _params[_params.length - 2];
    if (parseInt(_params[_params.length - 4]) <= 4) {
      window.open(
        `/report/${_obj.clickLevel}/${_obj.clickedWidgetId}/${
          _obj.clickedOnValue
        }/${window.btoa(JSON.stringify(_list))}`,
        "_blank"
      );
    }
  }
  checkNoData() {
    if (!this.props.drilldown_data.length) {
      return <h2 className="h2nodata">No Data to display</h2>;
    }
  }
  bindTableHeader() {
    if (this.props.drilldown_data.length) {
      return this.props.drilldown_data[0].gridColumns.map((_head, _index) => {
        return <th class="border border-b-2 whitespace-no-wrap">{_head}</th>;
      });
    }
  }
  bindTableBody() {
    if (this.props.drilldown_data.length) {
      return this.props.drilldown_data[0].gridData.map((_head, _index) => {
        return (
          <tr onClick={(e) => this.drillDown(e, _head)}>
            {_head.map((_body) => {
              return <td class="border">{_body}</td>;
            })}
          </tr>
        );
      });
    }
  }
  downloadReport() {
    //this.props.DOWNLOAD_REPORT_DETAILS(this.state.reportId);
    if (this.props.drilldown_data.length) {
      let _col =
        this.props.drilldown_data[0].gridColumns.map((e) => e).join(",") + "\n";
      let csvContent =
        "data:text/csv;charset=utf-8," +
        this.state.header +
        " Transactions - Level " +
        this.state.level +
        "\n\n" +
        _col +
        this.props.drilldown_data[0].gridData
          .map((e) => e.join(","))
          .join("\n");
      var encodedUri = encodeURI(csvContent);
      var link = document.createElement("a");
      link.setAttribute("href", encodedUri);
      link.setAttribute(
        "download",
        `${
          this.props.drilldown_data[0].clickedOnValue +
          "_Level_" +
          this.state.level +
          new Date().getTime()
        }.csv`
      );
      document.body.appendChild(link); // Required for FF

      link.click();
    }
  }
  sendMailPop(_flag) {
    this.setState({
      send_mail_pop: _flag,
      reportDeliveryMode: "email",
    });
  }
  logout() {
    window.location.href = "/";
  }
  previous(_flag) {
    if (!_flag) {
      //single
      if (this.state.startIndex <= 0) return;
      else
        this.setState({
          startIndex: this.state.startIndex - 10,
          endIndex: this.state.endIndex - 10,
        });
    } else {
      if (this.state.startIndex <= 0) return;
      else
        this.setState({
          startIndex: 0,
          endIndex: 11,
        });
    }
  }
  next(_flag) {
    if (!_flag) {
      //single
      if (this.state.endIndex >= this.props.drilldown_data[0].gridData.length)
        return;
      else
        this.setState({
          startIndex: this.state.startIndex + 10,
          endIndex: this.state.endIndex + 10,
        });
    } else {
      if (this.state.endIndex >= this.props.drilldown_data[0].gridData.length)
        return;
      else
        this.setState({
          startIndex: this.props.drilldown_data[0].gridData.length - 10,
          endIndex: this.props.drilldown_data[0].gridData.length,
        });
    }
  }
  delEmail(_email, _obj) {
    var _oriEmail = this.state[_obj];
    let _index = -1;
    _oriEmail.split(";").filter((xx, ii) => {
      if (xx === _email) _index = ii;
    });
    if (_index !== -1) {
      var _splitemail = _oriEmail.split(";");
      _splitemail.splice(_index, 1);
      this.setState((prevState) => {
        let _tempField = Object.assign({}, prevState);
        _tempField[_obj] = _splitemail.join(";");
        return _tempField;
      });
    }
  }
  render() {
    return (
      <>
        <div className="app">
          <div className="border border-theme-24 -mt-10 md:-mt-5 -mx-3 sm:-mx-8 px-3 sm:px-8 pt-3 md:pt-0 mb-10">
            <div className="top-bar-boxed flex items-center">
              <a href="" className="-intro-x hidden md:flex">
                <img
                  alt="Midone Tailwind HTML Admin Template"
                  className="w-6"
                  src={logo}
                />
              </a>
            </div>
          </div>

          <div className="content">
            <div className="intro-y flex items-center mt-8">
              <h2 className="text-lg font-medium mr-auto"> </h2>
            </div>
            <div className="intro-y grid grid-cols-12 ">
              <div className="col-span-12 lg:col-span-12">
                <div className="intro-y box">
                  <div className="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                    <h1
                      className="font-medium text-base mr-auto text-4xl "
                      style={{ color: "#1C3FAA" }}
                    >
                      {this.state.header} Transactions - Level{" "}
                      {this.state.level}
                    </h1>
                    <button
                      className="report_btn btn_mail"
                      onClick={() => this.sendMailPop(true)}
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
                        class="feather feather-mail mx-auto"
                      >
                        <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
                        <polyline points="22,6 12,13 2,6"></polyline>
                      </svg>
                      Send Mail/SMS
                    </button>
                    <button
                      className="report_btn btn_download"
                      onClick={() => this.downloadReport()}
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
                        class="feather feather-file-text mx-auto"
                      >
                        <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path>
                        <polyline points="14 2 14 8 20 8"></polyline>
                        <line x1="16" y1="13" x2="8" y2="13"></line>
                        <line x1="16" y1="17" x2="8" y2="17"></line>
                        <polyline points="10 9 9 9 8 9"></polyline>
                      </svg>
                      Download
                    </button>
                  </div>
                  <div className="p-5" id="vertical-bar-chart">
                    <div className="preview">
                      <div class="overflow-x-auto dataTables_wrapper">
                        {" "}
                        {this.checkNoData()}
                        <table class="table">
                          {" "}
                          <thead>
                            {" "}
                            <tr class="text-white">
                              {" "}
                              {this.bindTableHeader()}
                            </tr>{" "}
                          </thead>{" "}
                          <tbody> {this.bindTableBody()}</tbody>{" "}
                        </table>{" "}
                        <div
                          className={`dataTables_info ${
                            this.state.pagination ? "show" : "hide"
                          }`}
                          id="DataTables_Table_0_info"
                          role="status"
                          aria-live="polite"
                        >
                          Showing {this.state.startIndex + 1} to{" "}
                          {this.state.endIndex - 1} of {this.state.gridlength}{" "}
                          entries
                        </div>
                        <div
                          className={`dataTables_paginate paging_simple_numbers ${
                            this.state.pagination ? "show" : "hide"
                          }`}
                          id="DataTables_Table_0_paginate"
                        >
                          <a
                            class="paginate_button previous disabled"
                            aria-controls="DataTables_Table_0"
                            data-dt-idx="0"
                            tabindex="-1"
                            onClick={() => this.previous(true)}
                          >
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              width="18"
                              height="18"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              stroke-width="1.5"
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              class="feather feather-skip-back mx-auto"
                            >
                              <polygon points="19 20 9 12 19 4 19 20"></polygon>
                              <line x1="5" y1="19" x2="5" y2="5"></line>
                            </svg>
                          </a>
                          <a
                            class="paginate_button next disabled"
                            aria-controls="DataTables_Table_0"
                            data-dt-idx="2"
                            tabindex="-1"
                            onClick={() => this.previous(false)}
                          >
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              width="18"
                              height="18"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              stroke-width="1.5"
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              class="feather feather-play mx-auto"
                              style={{ transform: "rotate(180deg)" }}
                            >
                              <polygon points="5 3 19 12 5 21 5 3"></polygon>
                            </svg>
                          </a>
                          <span>
                            <a
                              class="paginate_button current"
                              aria-controls="DataTables_Table_0"
                              data-dt-idx="1"
                              tabindex="0"
                            >
                              {Math.round((this.state.endIndex - 1) / 10)}
                            </a>
                            <p>...</p>

                            <a
                              class="paginate_button current"
                              aria-controls="DataTables_Table_0"
                              data-dt-idx="1"
                              tabindex="0"
                            >
                              {Math.round(this.state.gridlength / 10)}
                            </a>
                          </span>
                          <a
                            class="paginate_button next disabled"
                            aria-controls="DataTables_Table_0"
                            data-dt-idx="2"
                            tabindex="-1"
                            onClick={() => this.next(false)}
                          >
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              width="18"
                              height="18"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              stroke-width="1.5"
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              class="feather feather-play mx-auto"
                            >
                              <polygon points="5 3 19 12 5 21 5 3"></polygon>
                            </svg>
                          </a>
                          <a
                            class="paginate_button next disabled"
                            aria-controls="DataTables_Table_0"
                            data-dt-idx="2"
                            tabindex="-1"
                            onClick={() => this.next(true)}
                          >
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              width="18"
                              height="18"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              stroke-width="1.5"
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              class="feather feather-skip-forward mx-auto"
                            >
                              <polygon points="5 4 15 12 5 20 5 4"></polygon>
                              <line x1="19" y1="5" x2="19" y2="19"></line>
                            </svg>
                          </a>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div
          className={`modal ${
            this.state.send_mail_pop ? "show model_show" : ""
          }`}
        >
          {" "}
          <div class="modal__content widget_model_content validate-form sendsmsform">
            <div class="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200">
              <h2 class="font-medium text-base mr-auto">Send Mail/SMS</h2>

              <div class="w-full sm:w-auto flex items-center sm:ml-auto mt-3 sm:mt-0">
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
                  className="feather feather-x mx-auto"
                  onClick={() => this.sendMailPop(false)}
                >
                  <line x1="18" y1="6" x2="6" y2="18"></line>
                  <line x1="6" y1="6" x2="18" y2="18"></line>
                </svg>
              </div>
            </div>
            <div class="flex flex-col sm:flex-row items-center">
              {" "}
              <label class="w-full sm:w-20 sm:text-right sm:mr-5"></label>{" "}
              <div class="flex flex-col sm:flex-row mt-2">
                <div class="flex items-center text-gray-700 dark:text-gray-500 mr-2">
                  {" "}
                  <input
                    type="radio"
                    name="reportDeliveryMode"
                    checked={
                      this.state.reportDeliveryMode === "email" ? true : false
                    }
                    onChange={(e) => this.handleChange(e)}
                    class="input border mr-2"
                  />
                  <label
                    class="cursor-pointer select-none"
                    for="horizontal-radio-chris-evans"
                    style={{ width: "4rem" }}
                  >
                    Email
                  </label>{" "}
                </div>
                <div class="flex items-center text-gray-700 dark:text-gray-500 mr-2 mt-2 sm:mt-0">
                  {" "}
                  <input
                    type="radio"
                    name="reportDeliveryMode"
                    checked={
                      this.state.reportDeliveryMode !== "email" ? true : false
                    }
                    onChange={(e) => this.handleChange(e)}
                    class="input border mr-2"
                  />
                  <label
                    class="cursor-pointer select-none"
                    for="horizontal-radio-liam-neeson"
                  >
                    SMS
                  </label>{" "}
                </div>
              </div>
            </div>{" "}
            <div
              className={`flex flex-col sm:flex-row items-center mt-3 ${
                this.state.reportDeliveryMode !== "email" ? "hide" : ""
              }`}
            >
              {" "}
              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                Email Option
              </label>{" "}
              <select
                class="select2 w-full input w-full border mt-2 flex-1"
                name="emailOption"
                value={this.state.emailOption}
                onChange={(e) => this.handleChange(e)}
              >
                <option value="1">Grid Users</option>
                <option value="2">Specific Users</option>
              </select>{" "}
            </div>{" "}
            <div
              className={`flex flex-col sm:flex-row items-center mt-3 ${
                this.state.reportDeliveryMode !== "email" ||
                this.state.emailOption === "1"
                  ? "hide"
                  : ""
              }`}
            >
              {" "}
              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                Recipient
              </label>{" "}
              <div className="custom_emailboxholder">
                <ul>
                  {this.state.emailReceipient.length
                    ? this.state.emailReceipient
                        .split(";")
                        .map((_email, _in) => {
                          if (_email.length)
                            return (
                              <li>
                                {_email}{" "}
                                <span
                                  onClick={() =>
                                    this.delEmail(_email, "emailReceipient")
                                  }
                                >
                                  x
                                </span>{" "}
                              </li>
                            );
                        })
                    : null}
                </ul>
                <input
                  type="text"
                  className={`input flex-1 ${
                    this.state.emailReceipientError ? "error" : ""
                  }`}
                  placeholder="Recipient Email"
                  name="emailReceipientDummy"
                  value={this.state.emailReceipientDummy}
                  onChange={(e) => this.handleChange(e)}
                  autocomplete="nope"
                  onBlur={(e) =>
                    this.bindEnteredEmail(e, "emailReceipientDummy")
                  }
                />{" "}
              </div>
            </div>{" "}
            <div
              className={`flex flex-col sm:flex-row items-center mt-3 ${
                this.state.reportDeliveryMode !== "email" ? "hide" : ""
              }`}
            >
              {" "}
              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                File Format
              </label>{" "}
              <select
                class="select2 w-full input w-full border mt-2 flex-1"
                name="fileFormat"
                value={this.state.fileFormat}
                onChange={(e) => this.handleChange(e)}
              >
                <option value="1">Excel</option>
                <option value="2">PDF</option>
              </select>{" "}
            </div>{" "}
            <div
              className={`flex flex-col sm:flex-row items-center mt-3 ${
                this.state.reportDeliveryMode === "email" ? "hide" : ""
              }`}
            >
              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                Phone No
              </label>{" "}
              <input
                type="text"
                className={`input w-full border mt-2 flex-1 ${
                  this.state.reportEmailError ? "error" : ""
                }`}
                placeholder="Recipient Phone Number"
                name="phoneNo"
                value={this.state.phoneNo}
                onChange={(e) => this.handleChange(e)}
              />{" "}
            </div>{" "}
            <div
              className={`flex flex-col sm:flex-row items-center mt-3 ${
                this.state.reportDeliveryMode === "email" ? "hide" : ""
              }`}
            >
              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                Default SMS Msg
              </label>{" "}
              <input
                type="text"
                className={`input w-full border mt-2 flex-1 ${
                  this.state.reportEmailError ? "error" : ""
                }`}
                placeholder="Default SMS Message"
                name="defaultSMS"
                value={this.state.defaultSMS}
                onChange={(e) => this.handleChange(e)}
              />{" "}
            </div>{" "}
            <div
              className={`flex flex-col sm:flex-row items-center mt-3 ${
                this.state.reportDeliveryMode === "email" ? "hide" : ""
              }`}
            >
              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                Select Column Name
              </label>{" "}
              <select
                class="select2 w-full input w-full border mt-2 flex-1"
                name="gridColumnName"
                value={this.state.gridColumnName}
                onChange={(e) => this.handleChange(e)}
              >
                {this.buildDropdownOptions()}
              </select>{" "}
            </div>{" "}
            <div class="sm:ml-20 sm:pl-5 mt-5" style={{ float: "right" }}>
              {" "}
              <button
                type="button"
                class="button bg-theme-1 text-white"
                onClick={() => this.sendMailPop(false)}
              >
                Send
              </button>{" "}
              <button
                type="button"
                class="button bg-theme-1 text-white"
                onClick={() => this.sendMailPop(false)}
              >
                Cancel
              </button>{" "}
            </div>
          </div>
        </div>
      </>
    );
  }
}
const mapProperties = (state) => {
  return {
    drilldown_data: state.dashboardReducer.drilldownData,
  };
};
const dispatch_action = (dispatch) => {
  //console.log("userDetails.UserContext.firmId", userDetails.UserContext.firmId)

  return {
    GET_DASHBOARD_DRILLDOWN: (_state) =>
      dispatch(action_type._post_drilldowndashboardWidget(_state)),
    SEND_DASHBOARD_MAIL: (_state) =>
      dispatch(action_type._sendmail_drilldowndashboardWidget(_state)),
  };
};

export default connect(mapProperties, dispatch_action)(Report);
