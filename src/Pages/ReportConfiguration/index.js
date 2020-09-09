import React from "react";
import { connect } from "react-redux";
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import LookUpUtilities from "../../Common/Utility/LookUpDataMapping";
import logo from "../../dist/images/logo.png";
import * as action_type from "../../actions/reportconfig/reportManagment";
import user_logout from "../../dist/images/user_mark.svg";
import Notification from "../Notification";
import * as notify_action_type from "../../actions/notification/notifiyaction";
import DayPicker from "react-day-picker";
import DayPickerInput from "react-day-picker/DayPickerInput";

import "react-day-picker/lib/style.css";

class ReportConfiguration extends React.Component {
  constructor(props) {
    super(props);
    this.month_picker = React.createRef();
    this.hour = [
      0,
      1,
      2,
      3,
      4,
      5,
      6,
      7,
      8,
      9,
      10,
      11,
      12,
      13,
      14,
      15,
      16,
      17,
      18,
      19,
      20,
      21,
      22,
      23,
    ];
    this.hour12 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
    this.minute = [0, 15, 30, 45];
    this.month = [
      1,
      2,
      3,
      4,
      5,
      6,
      7,
      8,
      9,
      10,
      11,
      12,
      13,
      14,
      15,
      16,
      17,
      18,
      19,
      20,
      21,
      22,
      23,
      24,
      25,
      26,
      27,
      28,
      29,
      30,
      31,
    ];
    this.state = {
      modal: true,
      scheduler: false,
      userDetails: {},
      reportQuery: "",
      reportEmail: "",
      reportEmailDummy: "",
      reportFormat: "Excel",
      reportInterval: "1 Hour",
      reportId: -1,
      reportName: "",
      reportDeliveryMode: "email",
      reportSMSPhoneNumber: "",
      reportDefaultSMSMSG: "",
      reportWorkStartTime: "00:00",
      reportWorkEndTime: "00:00",
      reportSendFrequency: "Daily",
      reportSendTime: "00:00",
      reportSendFrequencyValue: "",
      daily_option: "",
      selected_week: "",
      selected_month: "",
      selected_year: "",
      isActive: true,
      report_dbStatus: false,
      reportConnectionString: "OracleConnectionString",
      reportNameError: false,
      reportQueryError: false,
      reportSchedulerSelectError: false,
      reportSMSPhoneNumberError: false,
      reportEmailError: false,
      reportError: false,
      reportSchedulerName: "-1",

      isAddScheduler: true,
      schedulerName: "",
      schedulerNameError: false,
      schedulerIdError: false,
      schedulerId: -1,
      startIndex: 0,
      endIndex: 11,
      pagination: false,
      gridlength: 0,
    };
  }
  componentDidMount() {
    try {
      let _userDetails = LookUpUtilities.LoginDetails();
      this.setState({
        userDetails: _userDetails,
      });
      this.props.GET_REPORT_NAMES();
      document.querySelector("body").addEventListener("click", function (e) {
        if (e.target.getAttribute("skip") === null)
          if (document.querySelector(".monthPicker-holder") !== null)
            document.querySelector(".monthPicker-holder").style.display =
              "none";
      });
    } catch (e) {
      console.log("");
    }
  }

  componentWillUnmount() {}
  componentWillReceiveProps(nextProps) {
    console.log(nextProps.reportSendTime);
    if (nextProps.report_dbStatus) {
      //this.props.GET_REPORT_NAMES();
      this.clear();
    }
    if (Object.keys(nextProps.schedulerObject).length > 1) {
      this.setState({
        schedulerName: nextProps.schedulerObject.schedulerName,
        schedulerId: nextProps.schedulerObject.schedulerId,
        reportWorkStartTime: nextProps.schedulerObject.schedulerWorkStartTime,
        reportWorkEndTime: nextProps.schedulerObject.schedulerWorkEndTime,
        reportSendTime: nextProps.schedulerObject.schedulerSendTime,
        reportSendFrequency: nextProps.schedulerObject.schedulerSendFrequency,
        reportSendFrequencyValue:
          nextProps.schedulerObject.schedulerSendFrequencyValue,
        daily_option:
          nextProps.schedulerObject.schedulerSendFrequency === "Daily"
            ? nextProps.schedulerObject.schedulerSendFrequencyValue
            : "",
        selected_week:
          nextProps.schedulerObject.schedulerSendFrequency === "Weekly"
            ? nextProps.schedulerObject.schedulerSendFrequencyValue
            : "",
        selected_month:
          nextProps.schedulerObject.schedulerSendFrequency === "Monthly"
            ? nextProps.schedulerObject.schedulerSendFrequencyValue
            : "",
        selected_year:
          nextProps.schedulerObject.schedulerSendFrequency === "Yearly"
            ? nextProps.schedulerObject.schedulerSendFrequencyValue
            : "",
      });
    }
    if (Object.keys(nextProps.report_object).length > 1) {
      this.setState({
        reportQuery: nextProps.report_object.reportQuery,
        reportEmail: nextProps.report_object.reportEmail,
        reportFormat: nextProps.report_object.reportFormat,
        reportInterval: nextProps.report_object.reportInterval,
        reportId: nextProps.report_object.reportId,
        reportName: nextProps.report_object.reportName,
        reportConnectionString: nextProps.report_object.reportConnectionString,
        reportDeliveryMode: nextProps.report_object.reportDeliveryMode,
        reportSMSPhoneNumber: nextProps.report_object.reportSMSPhoneNumber,
        reportDefaultSMSMSG: nextProps.report_object.reportDefaultSMSMSG,
        reportSchedulerName: nextProps.report_object.reportSchedulerName,
        isActive: nextProps.report_object.isActive,
      });
    }
    if (nextProps.report_details.length)
      if (nextProps.report_details[0].gridData.length > 20)
        this.setState({
          pagination: true,
          gridlength: nextProps.report_details[0].gridData.length,
        });
  }

  showAddWidget(flag) {
    this.clear();
    this.setState((prevState) => ({
      // prevState?
      modal: flag,
      scheduler: false,
    }));
    if (!flag) this.props.GET_REPORT_NAMES();
    else this.props.GET_SCHEDULER_NAMES();
  }
  showScheduler() {
    this.clear();
    this.setState((prevState) => ({
      // prevState?
      scheduler: true,
      modal: false,
    }));
  }
  logout() {
    window.location.href = "/";
  }
  clear() {
    this.setState({
      reportQuery: "",
      reportEmail: "",
      reportEmailDummy: "",
      reportFormat: "Excel",
      reportInterval: "1 Hour",
      reportId: -1,
      reportName: "",
      reportConnectionString: "OracleConnectionString",
      reportNameError: false,
      reportQueryError: false,
      reportSchedulerSelectError: false,
      reportSMSPhoneNumberError: false,
      reportEmailError: false,
      reportError: false,
      reportDeliveryMode: "email",
      reportSMSPhoneNumber: "",
      reportDefaultSMSMSG: "",
      reportWorkStartTime: "00:00",
      reportWorkEndTime: "00:00",
      reportSendFrequency: "Daily",
      reportSendTime: "00:00",
      reportSendFrequencyValue: "",
      daily_option: "",
      selected_week: "",
      selected_month: "",
      selected_year: "",
      isAddScheduler: true,
      schedulerName: "",
      schedulerNameError: false,
      schedulerIdError: false,
      schedulerId: -1,
      reportSchedulerName: "",
    });
    if (this.state.scheduler) this.props.CLEAR_SCHEDULER();
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
      } else if (_name === "isAddScheduler") {
        _tempField[_name] = _tempField[_name] === true ? false : true;
        if (!_tempField[_name]) {
          _tempField.schedulerId = "-1";
          this.props.GET_SCHEDULER_NAMES();
          // this.clear();
          //  _tempField[_name] = false;
        } else {
          this.clear();
        }
      } else if (_name === "schedulerId") {
        _tempField.schedulerId = _value;
        if (_value !== "-1") this.props.EDIT_SCHEDULER_NAME(_value);
      } else if (
        _name === "reportSendTime" ||
        _name === "reportWorkStartTime" ||
        _name === "reportWorkEndTime"
      ) {
        let _final_time = _tempField[_name];
        if (_op === "h") {
          _final_time = _value + ":" + _final_time.split(":")[1];
        } else {
          _final_time = _final_time.split(":")[0] + ":" + _value;
        }
        _tempField[_name] = _final_time;
      } else if (_name === "selected_week") {
        let _finalweek = _tempField[_name];
        _finalweek = _finalweek.split(",");
        if (_value) _finalweek.push(_op);
        else {
          let _in = _finalweek.indexOf(_op);
          if (_in !== -1) _finalweek.splice(_in, 1);
        }
        _tempField[_name] = _finalweek.join(",");
      } else if (_name === "reportEmailDummy") {
        _tempField[_name] = _value;
        if (_value.indexOf(";") !== -1) {
          if (_emailreg.test(_value.trim())) {
            _tempField["reportEmail"] = _tempField["reportEmail"].length
              ? _tempField["reportEmail"] + ";" + _value.trim()
              : _value.trim();
            _tempField["reportEmailDummy"] = "";
          } else {
            LookUpUtilities.SetNotification(true, "Invalid EmailId", 2);
            this.props.SET_NOTIFICATION();
          }
        }
      } else _tempField[_name] = _value;
      let _finalschdule = "";
      if (_tempField["reportSendFrequency"] === "Daily")
        _finalschdule = _tempField["daily_option"];
      else if (_tempField["reportSendFrequency"] === "Weekly")
        _finalschdule = _tempField["selected_week"];
      else if (_tempField["reportSendFrequency"] === "Monthly")
        _finalschdule = _tempField["selected_month"];
      else if (_tempField["reportSendFrequency"] === "Yearly")
        _finalschdule = _tempField["selected_year"];
      _tempField["reportSendFrequencyValue"] = _finalschdule;
      return _tempField;
    });
  };
  loadReportName() {
    if (this.props.report_name.length) {
      return this.props.report_name.map((xx) => {
        return <option value={xx.reportId}>{xx.reportName}</option>;
      });
    }
  }
  loadSchedulerNames() {
    if (this.props.schedulerNames) {
      if (this.props.schedulerNames.length) {
        return this.props.schedulerNames.map((xx) => {
          return <option value={xx.schedulerId}>{xx.schedulerName}</option>;
        });
      }
    }
  }
  loadReportSchedulerNames() {
    if (this.props.schedulerNames) {
      if (this.props.schedulerNames.length) {
        return this.props.schedulerNames.map((xx) => {
          return <option value={xx.schedulerName}>{xx.schedulerName}</option>;
        });
      }
    }
  }
  delEmail(_email) {
    var _oriEmail = this.state.reportEmail;
    let _index = -1;
    _oriEmail.split(";").filter((xx, ii) => {
      if (xx === _email) _index = ii;
    });
    if (_index !== -1) {
      var _splitemail = _oriEmail.split(";");
      _splitemail.splice(_index, 1);
      this.setState({ reportEmail: _splitemail.join(";") });
    }
  }
  addReport() {
    const _emailreg = /^(\s?[^\s,]+@[^\s,]+\.[^\s,]+\s?,)*(\s?[^\s,]+@[^\s,]+\.[^\s,]+)$/;    
    let _emailstatus = false;
    if (
      this.state.reportDeliveryMode === "email" &&
      this.state.reportEmail.trim() !== ""
    ) {
      if (!_emailreg.test(this.state.reportEmail.trim())) _emailstatus = true;
    } else
      _emailstatus = this.state.reportDeliveryMode === "email" ? true : false;
    if (
      this.state.reportName.trim() === "" ||
      this.state.reportQuery.trim() === "" ||
      this.state.reportSchedulerName === "" ||
      this.state.reportSchedulerName === "-1" ||
      (this.state.reportDeliveryMode !== "email" &&
        this.state.reportSMSPhoneNumber === "") ||
      _emailstatus
    ) {
      this.setState({
        reportNameError: this.state.reportName.trim() === "" ? true : false,
        reportQueryError: this.state.reportQuery.trim() === "" ? true : false,
        reportSchedulerSelectError:
          this.state.reportSchedulerName === "" ||
          this.state.reportSchedulerName === "-1"
            ? true
            : false,
        reportSMSPhoneNumberError:
          this.state.reportDeliveryMode !== "email"
            ? this.state.reportSMSPhoneNumber === ""
              ? true
              : false
            : false,
        reportEmailError: _emailstatus,
        reportError: true,
      });
      LookUpUtilities.SetNotification(
        true,
        "Please fill the required field",
        2
      );
      this.props.SET_NOTIFICATION();
    } else {
      this.props.POST_REPORT_DETAILS(this.state);
      this.setState({
        report_dbStatus: true,
      });
      setTimeout(() => {
        this.setState({
          report_dbStatus: false,
        });
        this.clear();
        LookUpUtilities.SetNotification(true, "Report Updated Successfully", 1);
        this.props.SET_NOTIFICATION();
        this.showAddWidget(false);
      }, 2000);
    }
  }
  addscheduler() {
    if (
      this.state.schedulerName.trim() === "" ||
      (!this.state.isAddScheduler && this.state.schedulerId === "-1")
    ) {
      this.setState({
        schedulerNameError:
          this.state.schedulerName.trim() === "" ? true : false,
        schedulerIdError:
          this.state.isAddScheduler === "" || this.state.schedulerId === "-1"
            ? true
            : false,
      });
      LookUpUtilities.SetNotification(
        true,
        "Please fill the required field",
        2
      );
      this.props.SET_NOTIFICATION();
    } else {
      var _postScheduler = new Object();
      _postScheduler.schedulerId = this.state.schedulerId;
      _postScheduler.schedulerName = this.state.schedulerName;
      _postScheduler.schedulerWorkStartTime = this.state.reportWorkStartTime;
      _postScheduler.schedulerWorkEndTime = this.state.reportWorkEndTime;
      _postScheduler.schedulerSendFrequency = this.state.reportSendFrequency;
      _postScheduler.schedulerSendFrequencyValue = this.state.reportSendFrequencyValue;
      _postScheduler.schedulerSendTime = this.state.reportSendTime;
      this.props.POST_SCHEDULER_NAME(_postScheduler);
      this.clear();
    }
  }

  loadReportDetails(e) {
    let _id = parseInt(e.target.value);
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField.reportId = _id;
      return _tempField;
    });
    this.props.VIEW_REPORT_DETAILS(parseInt(e.target.value));
  }
  bindTableHeader() {
    if (this.props.report_details.length) {
      return this.props.report_details[0].gridColumns.map((_head, _index) => {
        return <th class="border border-b-2 whitespace-no-wrap">{_head}</th>;
      });
    }
  }
  bindTableBody() {
    if (this.props.report_details.length) {
      return this.props.report_details[0].gridData.map((_head, _index) => {
        return (
          <tr onClick={() => this.drillDown({ _head })}>
            {_head.map((_body) => {
              return <td class="border">{_body}</td>;
            })}
          </tr>
        );
      });
    }
  }
  editReport() {
    if (this.state.reportId !== -1) {
      this.props.EDIT_REPORT_DETAILS(this.state.reportId);
      this.showAddWidget(true);
    }
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
      if (this.state.endIndex >= this.props.report_details[0].gridData.length)
        return;
      else
        this.setState({
          startIndex: this.state.startIndex + 10,
          endIndex: this.state.endIndex + 10,
        });
    } else {
      if (this.state.endIndex >= this.props.report_details[0].gridData.length)
        return;
      else
        this.setState({
          startIndex: this.props.report_details[0].gridData.length - 10,
          endIndex: this.props.report_details[0].gridData.length,
        });
    }
  }
  downloadReport() {
    if (this.state.reportId !== -1) {
      //this.props.DOWNLOAD_REPORT_DETAILS(this.state.reportId);
      if (this.props.report_details.length) {
        let _col =
          this.props.report_details[0].gridColumns.map((e) => e).join(",") +
          "\n";
        let csvContent =
          "data:text/csv;charset=utf-8," +
          _col +
          this.props.report_details[0].gridData
            .map((e) => e.join(","))
            .join("\n");
        var encodedUri = encodeURI(csvContent);
        var link = document.createElement("a");
        link.setAttribute("href", encodedUri);
        link.setAttribute(
          "download",
          this.props.report_name.filter(
            (xx) => xx.reportId === this.state.reportId
          )[0].reportName +
            "_" +
            new Date().getTime() +
            ".csv"
        );
        document.body.appendChild(link); // Required for FF

        link.click();
      }
    }
  }
  checkNoData() {
    if (this.state.reportId !== -1) {
      if (!this.props.report_details.length) {
        return <h2 className="h2nodata">No Data to display</h2>;
      }
    }
  }
  day_selection(_month) {
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      let _prevmonth = _tempField.selected_month;
      _prevmonth =
        _prevmonth === ""
          ? []
          : _prevmonth.split(",").map((xx) => parseInt(xx, 10));
      let _monindex = -1;
      _monindex = _prevmonth.indexOf(parseInt(_month, 10));
      if (_monindex === -1) _prevmonth.push(parseInt(_month, 10));
      else _prevmonth.splice(_monindex, 1);
      _tempField.selected_month = _prevmonth
        .sort(function (a, b) {
          return a - b;
        })
        .join(",");
      _tempField.reportSendFrequencyValue = _prevmonth
        .sort(function (a, b) {
          return a - b;
        })
        .join(",");
      return _tempField;
    });
  }

  monthPicker() {
    return (
      <>
        <div className="monthPicker-holder" ref={this.month_picker} skip={true}>
          {this.month.map((_month) => {
            return (
              <p
                skip={"true"}
                onClick={() => this.day_selection(_month)}
                className={`${
                  this.state.selected_month
                    .split(",")
                    .filter((mm) => mm === _month.toString()).length
                    ? "sel_month"
                    : ""
                }`}
              >
                {_month}
              </p>
            );
          })}
        </div>
      </>
    );
  }
  showPicker(e) {
    console.log(e, this.month_picker);
    this.month_picker.current.style.top = e.pageY - 220 + "px";
    this.month_picker.current.style.left = e.pageX - 100 + "px";
    this.month_picker.current.style.display = "block";
  }
  bindEnteredEmail(e) {
    e.stopPropagation();
    e.preventDefault();
    const _emailreg = /^(\s?[^\s,]+@[^\s,]+\.[^\s,]+\s?,)*(\s?[^\s,]+@[^\s,]+\.[^\s,]+)$/;
    if (this.state.reportEmailDummy.trim().length) {
      if (_emailreg.test(this.state.reportEmailDummy.trim())) {
        var _combemail = this.state.reportEmail.length
          ? this.state.reportEmail + ";" + this.state.reportEmailDummy.trim()
          : this.state.reportEmailDummy.trim();
        this.setState({ reportEmail: _combemail, reportEmailDummy: "" });
      } else this.setState({ reportEmailDummy: "" });
    }
    return false;
  }
  render() {
    return (
      <>
        {this.monthPicker()}
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
                <span className="custom_logout" onClick={() => this.logout()}>
                  <img src={user_logout} className="custom_logout" />
                </span>

                <span className="userwelcome">
                  Welcome {this.state.userDetails.userid}
                </span>
              </div>
            </div>
          </div>
          <div class="flex">
            <nav class="side-nav">
              <ul>
                <li>
                  <a href="/dashboard" class="side-menu">
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
                <li className={"hidden"}>
                  <a
                    href="#"
                    className={`side-menu ${
                      this.state.modal ? "side-menu--active" : ""
                    }`}
                    onClick={() => this.showAddWidget(true, {})}
                  >
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
                        class="feather feather-plus-square mx-auto"
                      >
                        <rect
                          x="3"
                          y="3"
                          width="18"
                          height="18"
                          rx="2"
                          ry="2"
                        ></rect>
                        <line x1="12" y1="8" x2="12" y2="16"></line>
                        <line x1="8" y1="12" x2="16" y2="12"></line>
                      </svg>{" "}
                    </div>
                    <div class="side-menu__title"> Add Widgets </div>
                  </a>
                </li>

                <li>
                  <a href="/reportconfig" class="side-menu side-menu--active">
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
                <li
                  className={`${
                    this.state.userDetails.type !== 1 ? "hidden" : ""
                  }`}
                >
                  <a href="/userConfig" class="side-menu">
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
            <nav class="top-nav report_topnav">
              <ul>
                <li onClick={() => this.showAddWidget(false)}>
                  <a
                    className={`top-menu ${
                      !this.state.modal && !this.state.scheduler
                        ? "top-menu--active"
                        : ""
                    }`}
                  >
                    <div class="top-menu__icon">
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
                    <div class="top-menu__title"> View </div>
                  </a>
                </li>
                <li onClick={() => this.showAddWidget(true)}>
                  <a
                    className={`top-menu ${
                      this.state.modal ? "top-menu--active" : ""
                    }`}
                  >
                    <div class="top-menu__icon">
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
                        class="feather feather-settings mx-auto"
                      >
                        <circle cx="12" cy="12" r="3"></circle>
                        <path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z"></path>
                      </svg>
                    </div>
                    <div class="top-menu__title"> Config </div>
                  </a>
                </li>
                <li onClick={() => this.showScheduler(true)}>
                  <a
                    className={`top-menu ${
                      this.state.scheduler ? "top-menu--active" : ""
                    }`}
                  >
                    <div class="top-menu__icon">
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
                        class="feather feather-settings mx-auto"
                      >
                        <circle cx="12" cy="12" r="3"></circle>
                        <path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z"></path>
                      </svg>
                    </div>
                    <div class="top-menu__title"> Scheduler </div>
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
                  (this.state.scheduler ? "hidden" : this.state.modal)
                    ? "hidden"
                    : ""
                }`}
              >
                <div class="col-span-12 lg:col-span-12">
                  <div class="intro-y box">
                    <div class="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                      <h1 class="font-medium text-base mr-auto text-4xl ">
                        View Reports
                      </h1>
                    </div>
                    <div class="p-5" id="vertical-bar-chart">
                      <div
                        class="flex flex-col sm:flex-row items-center "
                        style={{ "margin-bottom": "50px" }}
                      >
                        {" "}
                        <label
                          class="w-full sm:w-20 sm:text-right sm:mr-5"
                          style={{ width: "8rem" }}
                        >
                          Select Report Name
                        </label>{" "}
                        <select
                          class="select2 w-full input border mt-2 "
                          style={{ width: "50%" }}
                          onChange={(e) => this.loadReportDetails(e)}
                        >
                          <option value="-1">
                            Select Report to View or Edit
                          </option>
                          {this.loadReportName()}
                        </select>{" "}
                        <button
                          style={{ padding: "0 35px" }}
                          onClick={() => this.editReport()}
                          className={`${
                            this.state.reportId == -1 ? "hide" : "show"
                          }`}
                        >
                          Edit
                        </button>
                        <button
                          className={`${
                            this.state.reportId == -1 ? "hide" : "show"
                          }`}
                          onClick={() => this.downloadReport()}
                        >
                          Download{" "}
                        </button>
                      </div>
                      {this.checkNoData()}
                      <div
                        className={`preview ${
                          !this.props.report_details.length ? "hidden" : ""
                        }`}
                      >
                        <div class="overflow-x-auto">
                          <h3
                            class="font-medium text-base mr-auto text-2xl "
                            style={{ "margin-bottom": "30px" }}
                          >
                            Detailed Report View
                          </h3>{" "}
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

              <div
                className={`intro-y grid grid-cols-12 ${
                  this.state.scheduler
                    ? "hidden"
                    : !this.state.modal
                    ? "hidden"
                    : ""
                }`}
              >
                <div class="col-span-12 lg:col-span-12">
                  <div class="intro-y box primary_background">
                    <div class="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                      <h1 class="font-medium text-base mr-auto text-4xl ">
                        Config Reports
                      </h1>
                    </div>
                    <div class="p-5" id="vertical-bar-chart">
                      <div class="preview validate-form">
                        <div class="overflow-x-auto">
                          {" "}
                          <div class="modal__content relative custom_model_content report_config_model">
                            <div class="intro-y box mt-5">
                              <div class="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200 dark:border-dark-5">
                                <h2 class="font-medium text-base mr-auto">
                                  General Report Config
                                </h2>
                                <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                  Active
                                </label>{" "}
                                <input
                                  type="checkbox"
                                  checked={this.state.isActive}
                                  name="isActive"
                                  onChange={(e) => this.handleChange(e)}
                                  class="input border mr-2"
                                />
                              </div>
                              <div class="p-5">
                                <div class="preview">
                                  <div class="flex flex-col sm:flex-row items-center">
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Name
                                    </label>{" "}
                                    <input
                                      type="text"
                                      className={`input w-full border mt-2 flex-1 ${
                                        this.state.reportNameError
                                          ? "error"
                                          : ""
                                      }`}
                                      placeholder="Report Name"
                                      name="reportName"
                                      value={this.state.reportName}
                                      onChange={(e) => this.handleChange(e)}
                                    />{" "}
                                  </div>{" "}
                                  <div class="flex flex-col sm:flex-row items-center">
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Query Connection String
                                    </label>{" "}
                                    <select
                                      class="select2 w-full input w-full border mt-2 flex-1"
                                      name="reportConnectionString"
                                      value={this.state.reportConnectionString}
                                      onChange={(e) => this.handleChange(e)}
                                    >
                                      <option value="PgAdmin4ConnectionString">
                                        PgAdmin4ConnectionString
                                      </option>
                                      <option value="OracleConnectionString">
                                        OracleConnectionString
                                      </option>
                                      <option value="SqlConnectionString">
                                        SqlConnectionString
                                      </option>
                                    </select>{" "}
                                  </div>{" "}
                                  <div class="flex flex-col sm:flex-row items-center">
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Query
                                    </label>{" "}
                                    <textarea
                                      className={`input w-full border mt-2 flex-1 ${
                                        this.state.reportQueryError
                                          ? "error"
                                          : ""
                                      }`}
                                      placeholder="Select statement"
                                      style={{ height: "150px" }}
                                      name="reportQuery"
                                      value={this.state.reportQuery}
                                      onChange={(e) => this.handleChange(e)}
                                    />{" "}
                                  </div>{" "}
                                  <div class="flex flex-col sm:flex-row items-center">
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Schedule Name
                                    </label>{" "}
                                    <select
                                      className={`select2 w-full input w-full border mt-2 flex-1 ${
                                        this.state.reportSchedulerSelectError
                                          ? "error"
                                          : ""
                                      }`}
                                      name="reportSchedulerName"
                                      value={this.state.reportSchedulerName}
                                      onChange={(e) => this.handleChange(e)}
                                    >
                                      <option value="-1">
                                        Select Schedule Type
                                      </option>
                                      {this.loadReportSchedulerNames()}
                                    </select>{" "}
                                  </div>{" "}
                                </div>
                              </div>
                            </div>
                            <div class="intro-y box mt-5">
                              <div class="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200 dark:border-dark-5">
                                <h2 class="font-medium text-base mr-auto">
                                  Delivery Configuration
                                </h2>
                              </div>
                              <div class="p-5">
                                <div class="preview">
                                  <div
                                    class="flex flex-col sm:flex-row items-center"
                                    style={{ height: "45px" }}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Delivery Options
                                    </label>{" "}
                                    <div className="radio_container">
                                      <span>Email</span>
                                      <input
                                        type="radio"
                                        name="reportDeliveryMode"
                                        checked={
                                          this.state.reportDeliveryMode ===
                                          "email"
                                            ? true
                                            : false
                                        }
                                        onChange={(e) => this.handleChange(e)}
                                        class="input border mr-2"
                                      />
                                      <span>SMS</span>
                                      <input
                                        type="radio"
                                        name="reportDeliveryMode"
                                        checked={
                                          this.state.reportDeliveryMode !==
                                          "email"
                                            ? true
                                            : false
                                        }
                                        onChange={(e) => this.handleChange(e)}
                                        class="input border mr-2"
                                      />
                                    </div>{" "}
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center ${
                                      this.state.reportDeliveryMode !== "email"
                                        ? "hide"
                                        : ""
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Email Id
                                    </label>{" "}
                                    <div className="custom_emailboxholder">
                                      <ul>
                                        {this.state.reportEmail.length
                                          ? this.state.reportEmail
                                              .split(";")
                                              .map((_email, _in) => {
                                                if (_email.length)
                                                  return (
                                                    <li>
                                                      {_email}{" "}
                                                      <span
                                                        onClick={() =>
                                                          this.delEmail(_email)
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
                                          this.state.reportEmailError
                                            ? "error"
                                            : ""
                                        }`}
                                        placeholder="Recipient Email Address"
                                        name="reportEmailDummy"
                                        autocomplete="nope"
                                        value={this.state.reportEmailDummy}
                                        onChange={(e) => this.handleChange(e)}
                                        onBlur={(e) => this.bindEnteredEmail(e)}
                                      />{" "}
                                    </div>
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center ${
                                      this.state.reportDeliveryMode === "email"
                                        ? "hide"
                                        : ""
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Phone No
                                    </label>{" "}
                                    <input
                                      type="text"
                                      className={`input w-full border mt-2 flex-1 ${
                                        this.state.reportSMSPhoneNumberError
                                          ? "error"
                                          : ""
                                      }`}
                                      placeholder="Recipient Phone Number"
                                      name="reportSMSPhoneNumber"
                                      value={this.state.reportSMSPhoneNumber}
                                      onChange={(e) => this.handleChange(e)}
                                    />{" "}
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center ${
                                      this.state.reportDeliveryMode === "email"
                                        ? "hide"
                                        : ""
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Default SMS Msg
                                    </label>{" "}
                                    <input
                                      type="text"
                                      className={`input w-full border mt-2 flex-1 ${
                                        this.state.reportEmailError
                                          ? "error"
                                          : ""
                                      }`}
                                      placeholder="Default SMS Message"
                                      name="reportDefaultSMSMSG"
                                      value={this.state.reportDefaultSMSMSG}
                                      onChange={(e) => this.handleChange(e)}
                                    />{" "}
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center ${
                                      this.state.reportDeliveryMode !== "email"
                                        ? "hide"
                                        : ""
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      File Format
                                    </label>{" "}
                                    <select
                                      class="select2 w-full input w-full border mt-2 flex-1"
                                      name="reportFormat"
                                      value={this.state.reportFormat}
                                      onChange={(e) => this.handleChange(e)}
                                    >
                                      <option value="1">Excel</option>
                                      <option value="2">PDF</option>
                                    </select>{" "}
                                  </div>{" "}
                                </div>
                              </div>
                            </div>

                            <div class="sm:ml-20 sm:pl-5 mt-5">
                              {" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                                style={{ float: "right" }}
                                onClick={() => this.addReport()}
                              >
                                Submit
                              </button>{" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                                style={{ float: "right", marginRight: "10px" }}
                                onClick={() => this.showAddWidget(false)}
                              >
                                Cancel
                              </button>{" "}
                            </div>
                          </div>{" "}
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div
                className={`intro-y grid grid-cols-12 ${
                  !this.state.scheduler ? "hidden" : ""
                }`}
              >
                <div class="col-span-12 lg:col-span-12">
                  <div class="intro-y box primary_background">
                    <div class="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                      <h1 class="font-medium text-base mr-auto text-4xl ">
                        Scheduler Config
                      </h1>
                    </div>
                    <div class="p-5" id="vertical-bar-chart">
                      <div class="preview validate-form">
                        <div class="overflow-x-auto">
                          {" "}
                          <div class="modal__content relative custom_model_content report_config_model">
                            <div class="intro-y box mt-5">
                              <div class="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200 dark:border-dark-5">
                                <h2 class="font-medium text-base mr-auto">
                                  Scheduler Configuration
                                </h2>
                              </div>
                              <div class="p-5">
                                <div class="preview">
                                  <div class="flex flex-col sm:flex-row items-center">
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5"></label>{" "}
                                    <div class="flex flex-col sm:flex-row mt-2">
                                      <div class="flex items-center text-gray-700 dark:text-gray-500 mr-2">
                                        {" "}
                                        <input
                                          type="radio"
                                          class="input border mr-2"
                                          name="isAddScheduler"
                                          checked={
                                            this.state.isAddScheduler
                                              ? true
                                              : false
                                          }
                                          onChange={(e) => this.handleChange(e)}
                                        />{" "}
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-radio-chris-evans"
                                          style={{ width: "4rem" }}
                                        >
                                          Add
                                        </label>{" "}
                                      </div>
                                      <div class="flex items-center text-gray-700 dark:text-gray-500 mr-2 mt-2 sm:mt-0">
                                        {" "}
                                        <input
                                          type="radio"
                                          class="input border mr-2"
                                          name="isAddScheduler"
                                          checked={
                                            !this.state.isAddScheduler
                                              ? true
                                              : false
                                          }
                                          onChange={(e) => this.handleChange(e)}
                                        />{" "}
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-radio-liam-neeson"
                                        >
                                          Edit
                                        </label>{" "}
                                      </div>
                                    </div>
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center  ${
                                      !this.state.isAddScheduler
                                        ? "show"
                                        : "hidden"
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Select Scheduler Name
                                    </label>{" "}
                                    <select
                                      className={`select2 w-full input w-full border mt-2 flex-1 ${
                                        this.state.schedulerIdError
                                          ? "error"
                                          : ""
                                      }`}
                                      name="schedulerId"
                                      value={this.state.schedulerId}
                                      onChange={(e) => this.handleChange(e)}
                                    >
                                      <option value="-1">
                                        Select Scheduler Name
                                      </option>
                                      {this.loadSchedulerNames()}
                                    </select>{" "}
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center `}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Scheduler Name
                                    </label>{" "}
                                    <input
                                      type="text"
                                      className={`input w-full border mt-2 flex-1 ${
                                        this.state.schedulerNameError
                                          ? "error"
                                          : ""
                                      }`}
                                      name="schedulerName"
                                      placeholder="Scheduler Name"
                                      value={this.state.schedulerName}
                                      onChange={(e) => this.handleChange(e)}
                                    />{" "}
                                  </div>{" "}
                                  <div class="flex flex-col sm:flex-row items-center">
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Work Start Time
                                    </label>{" "}
                                    <div class="sm:mt-2">
                                      {" "}
                                      <select
                                        class="input input--sm border mr-2"
                                        name="reportWorkStartTime"
                                        value={
                                          this.state.reportWorkStartTime.split(
                                            ":"
                                          )[0]
                                        }
                                        onChange={(e) =>
                                          this.handleChange(e, "h")
                                        }
                                      >
                                        {this.hour.map((ho) => {
                                          return <option>{ho}</option>;
                                        })}
                                      </select>{" "}
                                    </div>
                                    <div class="mt-2">
                                      {" "}
                                      <select
                                        class="input input--sm border mr-2"
                                        name="reportWorkStartTime"
                                        value={
                                          this.state.reportWorkStartTime.split(
                                            ":"
                                          )[1]
                                        }
                                        onChange={(e) =>
                                          this.handleChange(e, "m")
                                        }
                                      >
                                        {this.minute.map((ho) => {
                                          return <option>{ho}</option>;
                                        })}
                                      </select>{" "}
                                    </div>
                                  </div>
                                  <div class="flex flex-col sm:flex-row items-center">
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Work End Time
                                    </label>{" "}
                                    <div class="sm:mt-2">
                                      {" "}
                                      <select
                                        class="input input--sm border mr-2"
                                        name="reportWorkEndTime"
                                        value={
                                          this.state.reportWorkEndTime.split(
                                            ":"
                                          )[0]
                                        }
                                        onChange={(e) =>
                                          this.handleChange(e, "h")
                                        }
                                      >
                                        {this.hour.map((ho) => {
                                          return <option>{ho}</option>;
                                        })}
                                      </select>{" "}
                                    </div>
                                    <div class="mt-2">
                                      {" "}
                                      <select
                                        class="input input--sm border mr-2"
                                        name="reportWorkEndTime"
                                        value={
                                          this.state.reportWorkEndTime.split(
                                            ":"
                                          )[1]
                                        }
                                        onChange={(e) =>
                                          this.handleChange(e, "m")
                                        }
                                      >
                                        {this.minute.map((ho) => {
                                          return <option>{ho}</option>;
                                        })}
                                      </select>{" "}
                                    </div>
                                  </div>
                                  <div class="flex flex-col sm:flex-row items-center ">
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Frequency
                                    </label>{" "}
                                    <select
                                      class="select2 w-full input w-full border mt-2 flex-1"
                                      name="reportSendFrequency"
                                      value={this.state.reportSendFrequency}
                                      onChange={(e) => this.handleChange(e)}
                                    >
                                      <option value="Daily">Daily</option>
                                      <option value="Weekly">Weekly</option>
                                      <option value="Monthly">Monthly</option>
                                      <option value="Yearly">Yearly</option>
                                    </select>{" "}
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center ${
                                      this.state.reportSendFrequency === "Daily"
                                        ? "show"
                                        : "hide"
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Daily
                                    </label>{" "}
                                    <select
                                      class="select2 w-full input w-full border mt-2 flex-1"
                                      name="daily_option"
                                      value={this.state.daily_option}
                                      onChange={(e) => this.handleChange(e)}
                                    >
                                      {this.hour12.map((ho1) => {
                                        return (
                                          <option value={`${ho1} Hour`}>
                                            {ho1} Hour
                                          </option>
                                        );
                                      })}
                                      <option value="Once per day">
                                        Once per day
                                      </option>
                                    </select>{" "}
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center ${
                                      this.state.daily_option ===
                                        "Once per day" ||
                                      this.state.reportSendFrequency !== "Daily"
                                        ? "show"
                                        : "hide"
                                    }`}
                                  >
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Time
                                    </label>{" "}
                                    <div class="sm:mt-2">
                                      {" "}
                                      <select
                                        class="input input--sm border mr-2"
                                        name="reportSendTime"
                                        value={
                                          this.state.reportSendTime.split(
                                            ":"
                                          )[0]
                                        }
                                        onChange={(e) =>
                                          this.handleChange(e, "h")
                                        }
                                      >
                                        {this.hour.map((ho) => {
                                          return <option>{ho}</option>;
                                        })}
                                      </select>{" "}
                                    </div>
                                    <div class="mt-2">
                                      {" "}
                                      <select
                                        class="input input--sm border mr-2"
                                        name="reportSendTime"
                                        value={
                                          this.state.reportSendTime.split(
                                            ":"
                                          )[1]
                                        }
                                        onChange={(e) =>
                                          this.handleChange(e, "m")
                                        }
                                      >
                                        {this.minute.map((ho) => {
                                          return <option>{ho}</option>;
                                        })}
                                      </select>{" "}
                                    </div>
                                  </div>
                                  <div
                                    className={`flex flex-col sm:flex-row items-center mb-3 mt-1 ${
                                      this.state.reportSendFrequency ===
                                      "Weekly"
                                        ? "show"
                                        : "hide"
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Weekly
                                    </label>{" "}
                                    <div class="flex flex-col sm:flex-row mt-2">
                                      <div class="flex items-center text-gray-700 mr-2">
                                        {" "}
                                        <input
                                          data-target="#horizontal-form"
                                          class="show-code input input--switch border"
                                          type="checkbox"
                                          name="selected_week"
                                          checked={
                                            this.state.selected_week
                                              .split(",")
                                              .filter((xx) => xx === "Mon")
                                              .length
                                              ? true
                                              : false
                                          }
                                          onChange={(e) =>
                                            this.handleChange(e, "Mon")
                                          }
                                        />
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-checkbox-chris-evans"
                                          style={{ width: "2rem" }}
                                        >
                                          Mon
                                        </label>{" "}
                                      </div>
                                      <div class="flex items-center text-gray-700 mr-2 mt-2 sm:mt-0">
                                        {" "}
                                        <input
                                          data-target="#horizontal-form"
                                          class="show-code input input--switch border"
                                          type="checkbox"
                                          name="selected_week"
                                          checked={
                                            this.state.selected_week
                                              .split(",")
                                              .filter((xx) => xx === "Tue")
                                              .length
                                              ? true
                                              : false
                                          }
                                          onChange={(e) =>
                                            this.handleChange(e, "Tue")
                                          }
                                        />
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-checkbox-liam-neeson"
                                          style={{ width: "2rem" }}
                                        >
                                          Tue
                                        </label>{" "}
                                      </div>
                                      <div class="flex items-center text-gray-700 mr-2 mt-2 sm:mt-0">
                                        {" "}
                                        <input
                                          data-target="#horizontal-form"
                                          class="show-code input input--switch border"
                                          type="checkbox"
                                          name="selected_week"
                                          checked={
                                            this.state.selected_week
                                              .split(",")
                                              .filter((xx) => xx === "Wed")
                                              .length
                                              ? true
                                              : false
                                          }
                                          onChange={(e) =>
                                            this.handleChange(e, "Wed")
                                          }
                                        />
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-checkbox-daniel-craig"
                                          style={{ width: "2rem" }}
                                        >
                                          Wed
                                        </label>{" "}
                                      </div>
                                      <div class="flex items-center text-gray-700 mr-2 mt-2 sm:mt-0">
                                        {" "}
                                        <input
                                          data-target="#horizontal-form"
                                          class="show-code input input--switch border"
                                          type="checkbox"
                                          name="selected_week"
                                          checked={
                                            this.state.selected_week
                                              .split(",")
                                              .filter((xx) => xx === "Thu")
                                              .length
                                              ? true
                                              : false
                                          }
                                          onChange={(e) =>
                                            this.handleChange(e, "Thu")
                                          }
                                        />
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-checkbox-daniel-craig"
                                          style={{ width: "2rem" }}
                                        >
                                          Thu
                                        </label>{" "}
                                      </div>
                                      <div class="flex items-center text-gray-700 mr-2 mt-2 sm:mt-0">
                                        {" "}
                                        <input
                                          data-target="#horizontal-form"
                                          class="show-code input input--switch border"
                                          type="checkbox"
                                          name="selected_week"
                                          checked={
                                            this.state.selected_week
                                              .split(",")
                                              .filter((xx) => xx === "Fri")
                                              .length
                                              ? true
                                              : false
                                          }
                                          onChange={(e) =>
                                            this.handleChange(e, "Fri")
                                          }
                                        />
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-checkbox-daniel-craig"
                                          style={{ width: "2rem" }}
                                        >
                                          Fri
                                        </label>{" "}
                                      </div>
                                      <div class="flex items-center text-gray-700 mr-2 mt-2 sm:mt-0">
                                        {" "}
                                        <input
                                          data-target="#horizontal-form"
                                          class="show-code input input--switch border"
                                          type="checkbox"
                                          name="selected_week"
                                          checked={
                                            this.state.selected_week
                                              .split(",")
                                              .filter((xx) => xx === "Sat")
                                              .length
                                              ? true
                                              : false
                                          }
                                          onChange={(e) =>
                                            this.handleChange(e, "Sat")
                                          }
                                        />
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-checkbox-daniel-craig"
                                          style={{ width: "2rem" }}
                                        >
                                          Sat
                                        </label>{" "}
                                      </div>
                                      <div class="flex items-center text-gray-700 mr-2 mt-2 sm:mt-0">
                                        {" "}
                                        <input
                                          data-target="#horizontal-form"
                                          class="show-code input input--switch border"
                                          type="checkbox"
                                          name="selected_week"
                                          checked={
                                            this.state.selected_week
                                              .split(",")
                                              .filter((xx) => xx === "Sun")
                                              .length
                                              ? true
                                              : false
                                          }
                                          onChange={(e) =>
                                            this.handleChange(e, "Sun")
                                          }
                                        />
                                        <label
                                          class="cursor-pointer select-none"
                                          for="horizontal-checkbox-daniel-craig"
                                          style={{ width: "2rem" }}
                                        >
                                          Sun
                                        </label>{" "}
                                      </div>
                                    </div>
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center ${
                                      this.state.reportSendFrequency ===
                                      "Monthly"
                                        ? "show"
                                        : "hide"
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Monthly
                                    </label>{" "}
                                    <div class="relative w-56">
                                      <div class="absolute rounded-l w-10 h-full flex items-center justify-center text-gray-600 dark:bg-dark-1 dark:border-dark-4">
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
                                          class="feather feather-calendar mx-auto calender-icon-holder"
                                        >
                                          <rect
                                            x="3"
                                            y="4"
                                            width="18"
                                            height="18"
                                            rx="2"
                                            ry="2"
                                          ></rect>
                                          <line
                                            x1="16"
                                            y1="2"
                                            x2="16"
                                            y2="6"
                                          ></line>
                                          <line
                                            x1="8"
                                            y1="2"
                                            x2="8"
                                            y2="6"
                                          ></line>
                                          <line
                                            x1="3"
                                            y1="10"
                                            x2="21"
                                            y2="10"
                                          ></line>
                                        </svg>
                                      </div>
                                      <input
                                        type="text"
                                        className={`input w-full border mt-2 flex-1 monthtextIndent`}
                                        placeholder=""
                                        autoComplete="off"
                                        name="selected_month"
                                        readOnly={true}
                                        skip={true}
                                        value={this.state.selected_month}
                                        onClick={(e) => this.showPicker(e)}
                                      />
                                    </div>
                                  </div>{" "}
                                  <div
                                    className={`flex flex-col sm:flex-row items-center ${
                                      this.state.reportSendFrequency ===
                                      "Yearly"
                                        ? "show"
                                        : "hide"
                                    }`}
                                  >
                                    {" "}
                                    <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                      Yearly
                                    </label>{" "}
                                    <select
                                      class="select2 w-full input w-full border mt-2 flex-1"
                                      name="selected_year"
                                      value={this.state.selected_year}
                                      onChange={(e) => this.handleChange(e)}
                                    >
                                      <option value="1">Every 3 Months</option>
                                      <option value="2">Every 6 Months</option>
                                    </select>{" "}
                                  </div>{" "}
                                </div>
                              </div>
                            </div>

                            <div class="sm:ml-20 sm:pl-5 mt-5">
                              {" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                                style={{ float: "right" }}
                                onClick={() => this.addscheduler()}
                              >
                                Submit
                              </button>{" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                                style={{ float: "right", marginRight: "10px" }}
                                onClick={() => this.showAddWidget(false)}
                              >
                                Cancel
                              </button>{" "}
                            </div>
                          </div>{" "}
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <Notification></Notification>
      </>
    );
  }
}
const mapProperties = (state) => {
  return {
    report_name: state.reportReducer.report_name,
    report_dbStatus: state.reportReducer.report_dbStatus,
    report_details: state.reportReducer.report_details,
    report_object: state.reportReducer.report_object,
    reportSendTime: state.reportReducer.reportSendTime,
    schedulerNames: state.reportReducer.schedulerNames,
    schedulerObject: state.reportReducer.schedulerObject,
  };
};
const dispatch_action = (dispatch) => {
  //console.log("userDetails.UserContext.firmId", userDetails.UserContext.firmId)

  return {
    GET_REPORT_NAMES: () => dispatch(action_type._getReportNames()),
    POST_REPORT_DETAILS: (_state) =>
      dispatch(action_type._post_ReportDetails(_state)),
    VIEW_REPORT_DETAILS: (_state) =>
      dispatch(action_type._view_ReportDetails(_state)),
    EDIT_REPORT_DETAILS: (_id) =>
      dispatch(action_type._edit_ReportDetails(_id)),
    /* DOWNLOAD_REPORT_DETAILS: (_id) =>
      dispatch(action_type._download_ReportDetails(_id)),*/
    SET_NOTIFICATION: () => dispatch(notify_action_type._setNotify()),

    GET_SCHEDULER_NAMES: () => dispatch(action_type._get_scheduler_names()),
    POST_SCHEDULER_NAME: (_state) =>
      dispatch(action_type._post_SchedulerName(_state)),
    EDIT_SCHEDULER_NAME: (_id) =>
      dispatch(action_type._edit_SchedulerName(_id)),
    CLEAR_SCHEDULER: (_id) => dispatch(action_type._clear_Scheduler()),
    /*POST_DASHBOARD_WIDGETS: (_state) =>
    
      dispatch(action_type._post_dashboardWidget(_state)),
    DELETE_DASHBOARD_WIDGETS: (_id) =>
      dispatch(action_type._delete_dashboardWidget(_id)),*/
  };
};

export default connect(mapProperties, dispatch_action)(ReportConfiguration);
