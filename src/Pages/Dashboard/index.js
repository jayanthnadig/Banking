import React from "react";
import { connect } from "react-redux";
import ReactTooltip from "react-tooltip";
import LookUpUtilities from "../../Common/Utility/LookUpDataMapping";
import Notification from "../Notification";
import * as notify_action_type from "../../actions/notification/notifiyaction";
import Spinner from "../Notification/Spinner";
import * as action_type from "../../actions/dashboard/dashboardManagement";
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import logo from "../../dist/images/logo.png";
import user_logout from "../../dist/images/user_mark.svg";

class Dashboard extends React.Component {
  constructor(props) {
    super(props);
    this.select_picker = React.createRef();
    this.userPermiRef = React.createRef();
    this.state = {
      modal: false,
      delete_modal: false,
      userDetails: {},
      dashboardWidgetName: "",
      widgetId: -1,
      widgetSendEmail: true,
      dashboardChartType: "Line Chart",
      widgetQuery: "",
      dashboardUserPermission: "",
      dashboardUserPermissionError: false,
      dashboardEmailFormat: "",
      widgetSchedulerType: "0",
      widgetUseGridEmail: false,
      widgetSchedulerEmailIDs: "",
      widgetSchedulerEmailIDsDummy: "",
      widgetSchedulerEmailIDsError: false,
      dashbaordQueryL1: "",
      dashbaordQueryL2: "",
      dashbaordQueryL3: "",
      dashbaordQueryL4: "",
      level1ConnectionString: "",
      level1SchedulerType: "0",
      l1UseGridEmail: "1",
      l1SchedulerEmailIDs: "",
      l1SchedulerEmailIDsDummy: "",
      l1SchedulerEmailIDsError: false,
      level2ConnectionString: "",
      level2SchedulerType: "0",
      l2UseGridEmail: "1",
      l2SchedulerEmailIDs: "",
      l2SchedulerEmailIDsDummy: "",
      l2SchedulerEmailIDsError: false,
      l3UseGridEmail: "1",
      level3ConnectionString: "",
      level3SchedulerType: "0",
      l3SchedulerEmailIDs: "",
      l3SchedulerEmailIDsDummy: "",
      l3SchedulerEmailIDsError: false,
      l4UseGridEmail: "1",
      level4ConnectionString: "",
      level4SchedulerType: "0",
      widgetConnectionString: "",
      l4SchedulerEmailIDs: "",
      l4SchedulerEmailIDsDummy: "",
      l4SchedulerEmailIDsError: false,
      widgetNameError: false,
      widgetQueryError: false,
      widgetError: false,
      l1error: false,
      l2error: false,
      l3error: false,
      l4error: false,
      widgetEmailSubject: "",
      widgetEmailBody: "",
      l1EmailSubject: "",
      l1EmailBody: "",
      l2EmailSubject: "",
      l2EmailBody: "",
      l3EmailSubject: "",
      l3EmailBody: "",
      l4EmailSubject: "",
      l4EmailBody: "",

      selectOpen: false,
    };
  }
  componentDidMount() {
    let _userDetails = LookUpUtilities.LoginDetails();
    this.setState({
      userDetails: _userDetails,
    });
    this.props.GET_DASHBOARD_WIDGETS(_userDetails.userid);
    this.props.GET_ALL_DROPDOWNS();
    document.querySelector("body").addEventListener("click", function (e) {
      if (e.target.getAttribute("skip") === null)
        if (document.querySelector(".select2-container--open") !== null)
          document.querySelector(".select2-container--open").className =
            "select2-container select2-container--default select2-container--close";
    });
  }

  componentWillUnmount() {
    if (this.chart) {
      this.chart.dispose();
    }
    if (this.chart1) {
      this.chart1.dispose();
    }
  }
  componentWillReceiveProps(nextProps) {
    console.log(nextProps.widget_data);
    if (nextProps.widget_data.length) {
      setTimeout(() => {
        this.bindDatatoChart();
      }, 100);
      this.showAddWidget(false, {});
    }
    if (Object.keys(nextProps.singleDashboardWidget).length) {
      this.showAddWidget(true, nextProps.singleDashboardWidget);
    }
    //  this.bindDatatoChart()
  }
  showAddWidget(_flag, _obj) {
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField.modal = _flag;
      if (Object.keys(_obj).length) {
        _tempField.dashboardWidgetName = _obj.dashboardWidgetName;
        _tempField.widgetId = _obj.widgetId;
        _tempField.dashboardChartType = _obj.dashboardChartType;
        _tempField.dashboardUserPermission = _obj.dashboardUserPermission;
        _tempField.widgetSchedulerEmailIDs = _obj.widgetSchedulerEmailIDs;
        _tempField.widgetUseGridEmail = _obj.widgetUseGridEmail;
        _tempField.widgetSchedulerType = _obj.widgetSchedulerType;
        _tempField.dashboardEmailFormat = _obj.dashboardEmailFormat;
        _tempField.widgetQuery = _obj.widgetQuery;
        _tempField.dashbaordQueryL1 = _obj.dashbaordQueryL1;
        _tempField.level1SchedulerType = _obj.level1SchedulerType;
        _tempField.l1UseGridEmail = _obj.l1UseGridEmail.toString();
        _tempField.l1SchedulerEmailIDs = _obj.l1SchedulerEmailIDs;
        _tempField.dashbaordQueryL2 = _obj.dashbaordQueryL2;
        _tempField.level2SchedulerType = _obj.level2SchedulerType;
        _tempField.l2UseGridEmail = _obj.l2UseGridEmail.toString();
        _tempField.l2SchedulerEmailIDs = _obj.l2SchedulerEmailIDs;
        _tempField.dashbaordQueryL3 = _obj.dashbaordQueryL3;
        _tempField.level3SchedulerType = _obj.level3SchedulerType;
        _tempField.l3UseGridEmail = _obj.l3UseGridEmail.toString();
        _tempField.l3SchedulerEmailIDs = _obj.l3SchedulerEmailIDs;
        _tempField.dashbaordQueryL4 = _obj.dashbaordQueryL4;
        _tempField.level4SchedulerType = _obj.level4SchedulerType;
        _tempField.l4UseGridEmail = _obj.l4UseGridEmail.toString();
        _tempField.l4SchedulerEmailIDs = _obj.l4SchedulerEmailIDs;
        _tempField.widgetConnectionString = _obj.widgetConnectionString;
        _tempField.level1ConnectionString = _obj.level1ConnectionString;
        _tempField.level2ConnectionString = _obj.level2ConnectionString;
        _tempField.level3ConnectionString = _obj.level3ConnectionString;
        _tempField.level4ConnectionString = _obj.level4ConnectionString;
        _tempField.widgetEmailSubject = _obj.widgetEmailSubject;
        _tempField.widgetEmailBody = _obj.widgetEmailBody;
        _tempField.l1EmailSubject = _obj.l1EmailSubject;
        _tempField.l1EmailBody = _obj.l1EmailBody;
        _tempField.l2EmailSubject = _obj.l2EmailSubject;
        _tempField.l2EmailBody = _obj.l2EmailBody;
        _tempField.l3EmailSubject = _obj.l3EmailSubject;
        _tempField.l3EmailBody = _obj.l3EmailBody;
        _tempField.l4EmailSubject = _obj.l4EmailSubject;
        _tempField.l4EmailBody = _obj.l4EmailBody;
      } else {
        _tempField.dashboardWidgetName = "";
        _tempField.widgetId = -1;
        _tempField.dashboardChartType = "Pie Chart";
        _tempField.widgetQuery = "";
        _tempField.dashbaordQueryL1 = "";
        _tempField.dashbaordQueryL2 = "";
        _tempField.dashbaordQueryL3 = "";
        _tempField.dashbaordQueryL4 = "";
        _tempField.widgetUseGridEmail = false;
        _tempField.level1ConnectionString = "PgAdmin4ConnectionString";
        _tempField.level2ConnectionString = "PgAdmin4ConnectionString";
        _tempField.level3ConnectionString = "PgAdmin4ConnectionString";
        _tempField.level4ConnectionString = "PgAdmin4ConnectionString";
        _tempField.widgetConnectionString = "PgAdmin4ConnectionString";
        _tempField.l1UseGridEmail = "true";
        _tempField.l2UseGridEmail = "true";
        _tempField.l3UseGridEmail = "true";
        _tempField.l4UseGridEmail = "true";
        _tempField.level1SchedulerType = "0";
        _tempField.level2SchedulerType = "0";
        _tempField.level3SchedulerType = "0";
        _tempField.level4SchedulerType = "0";
        _tempField.widgetSchedulerType = "0";
        _tempField.widgetSchedulerEmailIDs = "";
        _tempField.l1SchedulerEmailIDs = "";
        _tempField.l2SchedulerEmailIDs = "";
        _tempField.l3SchedulerEmailIDs = "";
        _tempField.l4SchedulerEmailIDs = "";
        _tempField.dashboardUserPermission = "";
        _tempField.widgetNameError = false;
        _tempField.widgetQueryError = false;
        _tempField.dashboardUserPermissionError = false;
        _tempField.widgetError = false;
        _tempField.widgetSchedulerEmailIDsError = false;
        _tempField.l1SchedulerEmailIDsError = false;
        _tempField.l2SchedulerEmailIDsError = false;
        _tempField.l3SchedulerEmailIDsError = false;
        _tempField.l4SchedulerEmailIDsError = false;
        _tempField.widgetEmailSubject = "";
        _tempField.widgetEmailBody = "";
        _tempField.l1EmailSubject = "";
        _tempField.l1EmailBody = "";
        _tempField.l2EmailSubject = "";
        _tempField.l2EmailBody = "";
        _tempField.l3EmailSubject = "";
        _tempField.l3EmailBody = "";
        _tempField.l4EmailSubject = "";
        _tempField.l4EmailBody = "";
      }
      return _tempField;
    });
  }
  showEditPop(_id) {
    this.props.EDIT_DASHBOARD_WIDGET(_id);
  }
  addUserPermission(_user) {
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField.dashboardUserPermission = _tempField.dashboardUserPermission
        .length
        ? _tempField.dashboardUserPermission + "," + _user
        : _user;
      return _tempField;
    });
  }
  bindDropdownLookup(_type) {
    if (this.props.dashboardDropdown.length) {
      return this.props.dashboardDropdown[0][_type].map((dd) => {
        if (_type === "charts")
          return <option value={dd.chartName}>{dd.chartName}</option>;
        else if (_type === "connectionStrings")
          return (
            <option value={dd.dbConnectionName}>{dd.dbConnectionName}</option>
          );
        else if (_type === "schedulers")
          return <option value={dd.schedulerName}>{dd.schedulerName}</option>;
        else if (_type === "users") {
          if (this.state.dashboardUserPermission.length) {
            if (
              this.state.dashboardUserPermission
                .split(",")
                .filter((_u) => _u === dd).length === 0
            ) {
              return (
                <li
                  class="select2-results__option"
                  onClick={() => this.addUserPermission(dd)}
                >
                  {dd}
                </li>
              );
            }
          } else {
            return (
              <li
                class="select2-results__option"
                onClick={() => this.addUserPermission(dd)}
              >
                {dd}
              </li>
            );
          }
        }
      });
    }
  }
  showDeletePop(_flag, _obj) {
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField.delete_modal = _flag;
      if (Object.keys(_obj).length) {
        _tempField.widgetId = _obj.dashboardWidgetId;
      }
      return _tempField;
    });
  }
  deleteWidget() {
    this.setState({
      delete_modal: false,
    });
    this.props.DELETE_DASHBOARD_WIDGETS(this.state.widgetId);
  }
  logout() {
    this.props.USER_LOGOUT();
    window.location.href = "/";
  }
  loadCustomSelect = (e) => {
    let _target = e;
    this.select_picker.current.style.top =
      this.userPermiRef.current.offsetTop - 90 + "px";
    this.select_picker.current.style.left =
      this.userPermiRef.current.offsetLeft - 0 + "px";
    this.select_picker.current.className =
      "select2-container select2-container--default select2-container--open";
  };
  removeUser(_user) {
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      let _index = -1;
      _tempField.dashboardUserPermission.split(",").filter((_u, _i) => {
        if (_u === _user) _index = _i;
      });
      if (_index !== -1) {
        let _namearr = _tempField.dashboardUserPermission.split(",");
        _namearr.splice(_index, 1);
        _tempField.dashboardUserPermission = _namearr.join(",");
      }
      return _tempField;
    });
  }
  loadUserPermission = () => {
    if (this.state.dashboardUserPermission.length) {
      return this.state.dashboardUserPermission.split(",").map((_user) => {
        return (
          <li class="select2-selection__choice" title={_user}>
            <span
              class="select2-selection__choice__remove"
              onClick={() => this.removeUser(_user)}
            >
              {" "}
              ×{" "}
            </span>{" "}
            {_user}{" "}
          </li>
        );
      });
    }
  };
  handleChange = (e) => {
    const _emailreg = /^(\s?[^\s,]+@[^\s,]+\.[^\s,]+\s?,)*(\s?[^\s,]+@[^\s,]+\.[^\s,]+)$/;
    let _name = e.target.name;
    let _value = e.target.value;
    let _type = e.target.type;
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      if (_name.includes("Dummy")) {
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
  addWidget = () => {
    console.log(this.state.dashboardWidgetName);
    if (
      this.state.dashboardWidgetName.trim() === "" ||
      this.state.widgetQuery.trim() === "" ||
      this.state.dashboardUserPermission === "" ||
      (this.state.widgetUseGridEmail !== "true" &&
        this.state.widgetSchedulerEmailIDs === "") ||
      (this.state.l1UseGridEmail !== "true" &&
        this.state.l1SchedulerEmailIDs === "") ||
      (this.state.l2UseGridEmail !== "true" &&
        this.state.l2SchedulerEmailIDs === "") ||
      (this.state.l3UseGridEmail !== "true" &&
        this.state.l3SchedulerEmailIDs === "") ||
      (this.state.l4UseGridEmail !== "true" &&
        this.state.l4SchedulerEmailIDs === "")
    ) {
      this.setState({
        widgetNameError:
          this.state.dashboardWidgetName.trim() === "" ? true : false,
        widgetQueryError: this.state.widgetQuery.trim() === "" ? true : false,
        dashboardUserPermissionError:
          this.state.dashboardUserPermission.trim() === "" ? true : false,
        widgetSchedulerEmailIDsError:
          this.state.widgetUseGridEmail !== "true" &&
          this.state.widgetSchedulerEmailIDs === ""
            ? true
            : false,
        l1SchedulerEmailIDsError:          
          this.state.l1UseGridEmail !== "true" &&
          this.state.l1SchedulerEmailIDs === ""
            ? true
            : false,
        l2SchedulerEmailIDsError:          
          this.state.l2UseGridEmail !== "true" &&
          this.state.l2SchedulerEmailIDs === ""
            ? true
            : false,
        l3SchedulerEmailIDsError:         
          this.state.l3UseGridEmail !== "true" &&
          this.state.l3SchedulerEmailIDs === ""
            ? true
            : false,
        l4SchedulerEmailIDsError:        
          this.state.l4UseGridEmail !== "true" &&
          this.state.l4SchedulerEmailIDs === ""
            ? true
            : false,
        widgetError: true,
      });
      LookUpUtilities.SetNotification(
        true,
        "Please fill the required fields",
        2
      );
      this.props.SET_NOTIFICATION();
    } else {
      this.setState({
        widgetError: false,
      });
      this.props.POST_DASHBOARD_WIDGETS(JSON.parse(JSON.stringify(this.state)));
    }
  };
  bindDatatoChart() {
    if (this.props.widget_data.length) {
      return this.props.widget_data.map((_chartData, index) => {
        if (_chartData.dashbaordWidgetData.length) {
          if (_chartData.dashboardWidgetType == "Pie Chart") {
            let chart = am4core.create(
              "chartdiv" + index + "",
              am4charts.PieChart
            );
            chart.data = _chartData.dashbaordWidgetData;

            this.chart = chart;

            let pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "count";
            pieSeries.dataFields.category = "name";

            pieSeries.slices.template.events.on(
              "hit",
              function (ev) {
                //console.log(ev.target.dataItem.dataContext.name);
                window.open(
                  `/report/${1}/${_chartData.dashboardWidgetId}/${
                    ev.target.dataItem.dataContext.name
                  }/null`,
                  "_blank"
                );
              },
              this
            );
          } else if (_chartData.dashboardWidgetType == "Bar Chart") {
            let chart = am4core.create(
              "chartdiv" + index + "",
              am4charts.XYChart
            );
            chart.data = _chartData.dashbaordWidgetData;

            this.chart = chart;
            let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "name";
            categoryAxis.title.text = "name";

            let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.title.text = "";

            let series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueY = "count";
            series.dataFields.categoryX = "name";

            series.columns.template.events.on(
              "hit",
              function (ev) {
                console.log(ev.target.dataItem.dataContext.name);
                window.open(
                  `/report/${1}/${_chartData.dashboardWidgetId}/${
                    ev.target.dataItem.dataContext.name
                  }/null`,
                  "_blank"
                );
              },
              this
            );
          }
        } else {
          return <h2>No Data to display</h2>;
        }
      });
    }
  }
  bindChartContainer() {
    if (this.props.widget_data.length) {
      return this.props.widget_data.map((_chartData, index) => {
        return (
          <div className="col-span-12 lg:col-span-6">
            <div className="intro-y box">
              <div className="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200">
                <h2 className="font-medium text-base mr-auto custom-h2width text-2xl">
                  {_chartData.dashboardWidgetName}
                </h2>
                <div className="range-holder">
                  <select
                    data-hide-search="true"
                    className="select2 input w-full border mt-2 flex-1 custom-select2"
                  ></select>
                  <select
                    data-hide-search="true"
                    className="select2 input w-full border mt-2 flex-1 custom-select2"
                  ></select>
                </div>

                <div className="w-full sm:w-auto flex items-center sm:ml-auto mt-3 sm:mt-0">
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
                    className={`feather feather-edit mx-auto custom_refresh ${
                      !this.state.userDetails.isEditRights ? "hidden" : ""
                    }`}
                    onClick={() =>
                      this.showEditPop(_chartData.dashboardWidgetId)
                    }
                  >
                    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                  </svg>
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
                    className={`feather feather-trash-2 mx-auto ${
                      !this.state.userDetails.isDeleteRights ? "hidden" : ""
                    }`}
                    onClick={() => this.showDeletePop(true, _chartData)}
                  >
                    <polyline points="3 6 5 6 21 6"></polyline>
                    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                    <line x1="10" y1="11" x2="10" y2="17"></line>
                    <line x1="14" y1="11" x2="14" y2="17"></line>
                  </svg>
                </div>
              </div>
              <div className="p-5">
                <div className="preview">
                  <div
                    id={`chartdiv${index}`}
                    style={{ width: "100%", height: "500px" }}
                  ></div>
                </div>
                <div className="source-code hidden">
                  <button
                    data-target="#copy-vertical-bar-chart"
                    className="copy-code button button--sm border flex items-center text-gray-700"
                  >
                    {" "}
                    <i data-feather="file" className="w-4 h-4 mr-2"></i> Copy
                    code{" "}
                  </button>
                  <div className="overflow-y-auto h-64 mt-3">
                    <pre
                      className="source-preview"
                      id="copy-vertical-bar-chart"
                    >
                      {" "}
                      <code className="text-xs p-0 rounded-md html pl-5 pt-8 pb-4 -mb-10 -mt-10">
                        {" "}
                        HTMLOpenTagcanvas
                        id=&quot;vertical-bar-chart-widget&quot;
                        height=&quot;200&quot;HTMLCloseTagHTMLOpenTag/canvasHTMLCloseTag{" "}
                      </code>{" "}
                    </pre>
                  </div>
                </div>
              </div>
            </div>
          </div>
        );
      });
    }
  }
  render() {
    return (
      <>
        <Spinner></Spinner>
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

              {/* <div className="-intro-x breadcrumb breadcrumb--light mr-auto">
                {" "}
                <a href="" className="">
                  Application
                </a>{" "}
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
                  class="feather feather-chevron-right breadcrumb__icon"
                >
                  <polyline points="9 18 15 12 9 6"></polyline>
                </svg>{" "}
                <a href="" className="breadcrumb--active">
                  Dashboard
                </a>{" "}
              </div> */}
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
                  <a
                    href="/dashboard"
                    className={`side-menu ${
                      !this.state.modal ? "side-menu--active" : ""
                    }`}
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
                        class="feather feather-home"
                      >
                        <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path>
                        <polyline points="9 22 9 12 15 12 15 22"></polyline>
                      </svg>{" "}
                    </div>
                    <div class="side-menu__title"> Dashboard </div>
                  </a>
                </li>
                <li
                  className={`${
                    !this.state.userDetails.isAddRights ? "hidden" : ""
                  }`}
                  onClick={() => this.showAddWidget(true, {})}
                >
                  <a
                    className={`side-menu ${
                      this.state.modal ? "side-menu--active" : ""
                    }`}
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
                    <div class="side-menu__title"> Add Widget </div>
                  </a>
                </li>

                <li>
                  <a href="/reportconfig" class="side-menu">
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
            <div className="content">
              <div className="intro-y flex items-center mt-8">
                <h2 className="text-lg font-medium mr-auto">
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
                    className="feather feather-plus-circle mx-auto custom_pluscircle hidden"
                    onClick={() => this.showAddWidget(true, {})}
                  >
                    <circle cx="12" cy="12" r="10"></circle>
                    <line x1="12" y1="8" x2="12" y2="16"></line>
                    <line x1="8" y1="12" x2="16" y2="12"></line>
                  </svg>
                </h2>
                <span className="custom-refresh-text">Refresh</span>
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
                  class="feather feather-refresh-cw custom-mainrefresh"
                  onClick={() => this.props.GET_DASHBOARD_WIDGETS("Admin")}
                >
                  <polyline points="23 4 23 10 17 10"></polyline>
                  <polyline points="1 20 1 14 7 14"></polyline>
                  <path d="M3.51 9a9 9 0 0 1 14.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0 0 20.49 15"></path>
                </svg>
              </div>
              <div
                className={`intro-y grid grid-cols-12 gap-6 mt-5 ${
                  this.state.modal ? "hidden" : ""
                }`}
              >
                {this.bindChartContainer()}
              </div>
              <div>
                <div
                  className={`p-10 ${
                    this.state.modal ? "show model_show" : "hidden"
                  }`}
                  id="button-modal-preview"
                >
                  {" "}
                  <div class="modal__content relative widget_model_content validate-form intro-y box ">
                    <div class="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200">
                      <h2 class="font-medium text-base mr-auto">
                        {this.state.widgetId == -1 ? "Add" : "Edit"} Widget
                      </h2>

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
                          onClick={() => this.showAddWidget(false, {})}
                        >
                          <line x1="18" y1="6" x2="6" y2="18"></line>
                          <line x1="6" y1="6" x2="18" y2="18"></line>
                        </svg>
                      </div>
                    </div>
                    <div class="grid grid-cols-12 gap-6 mt-5">
                      <div class="intro-y col-span-12 lg:col-span-6">
                        <div class="intro-y">
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Name
                            </label>{" "}
                            <input
                              type="text"
                              className={`input w-full border mt-2 flex-1 ${
                                this.state.widgetNameError ? "error" : ""
                              }`}
                              name="dashboardWidgetName"
                              placeholder="Widget Name"
                              required
                              value={this.state.dashboardWidgetName}
                              onChange={(e) => this.handleChange(e)}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Widget Type
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="dashboardChartType"
                              value={this.state.dashboardChartType}
                              onChange={(e) => this.handleChange(e)}
                            >
                              {this.bindDropdownLookup("charts")}
                            </select>{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label
                              class="w-full sm:w-20 sm:text-right sm:mr-5"
                              ref={this.userPermiRef}
                            >
                              Users Permission
                            </label>{" "}
                            <div class="select-preview" skip={true}>
                              <span
                                className={`select2 select2-container select2-container--default select2-container--below select2-container--focus ${
                                  this.state.dashboardUserPermissionError
                                    ? "error"
                                    : ""
                                }`}
                                dir="ltr"
                                data-select2-id="8"
                              >
                                <span class="selection">
                                  <span
                                    className={`select2-selection select2-selection--multiple ${
                                      this.state.dashboardUserPermissionError
                                        ? "error"
                                        : ""
                                    }`}
                                    role="combobox"
                                    aria-haspopup="true"
                                    aria-expanded="false"
                                    tabindex="-1"
                                    aria-disabled="false"
                                  >
                                    <ul class="select2-selection__rendered">
                                      {this.loadUserPermission()}

                                      <li class="select2-search select2-search--inline">
                                        <input
                                          skip={true}
                                          class="select2-search__field"
                                          type="search"
                                          tabindex="0"
                                          autocomplete="off"
                                          autocorrect="off"
                                          autocapitalize="none"
                                          spellcheck="false"
                                          role="searchbox"
                                          aria-autocomplete="list"
                                          placeholder="Select Users"
                                          onClick={(e) =>
                                            this.loadCustomSelect(e)
                                          }
                                        />
                                      </li>
                                    </ul>
                                  </span>
                                </span>
                                <span
                                  class="dropdown-wrapper"
                                  aria-hidden="true"
                                ></span>
                              </span>
                              <span
                                ref={this.select_picker}
                                skip={true}
                                className={`select2-container select2-container--default select2-container--close`}
                              >
                                <span
                                  class="select2-dropdown select2-dropdown--below"
                                  dir="ltr"
                                >
                                  <span class="select2-results">
                                    <ul
                                      class="select2-results__options"
                                      role="listbox"
                                      aria-multiselectable="true"
                                      aria-expanded="true"
                                      aria-hidden="false"
                                    >
                                      {this.bindDropdownLookup("users")}
                                    </ul>
                                  </span>
                                </span>
                              </span>
                            </div>
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Attachment Format
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="dashboardEmailFormat"
                              value={this.state.dashboardEmailFormat}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="Excel">Excel</option>
                              <option value="PDF">PDF</option>
                            </select>{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Widget Connection String
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="widgetConnectionString"
                              value={this.state.widgetConnectionString}
                              onChange={(e) => this.handleChange(e)}
                            >
                              {this.bindDropdownLookup("connectionStrings")}
                            </select>{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Widget Scheduler Type
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="widgetSchedulerType"
                              value={this.state.widgetSchedulerType}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="0">Select Scheduler Type</option>
                              {this.bindDropdownLookup("schedulers")}
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 ${
                              this.state.widgetUseGridEmail === true
                                ? "hide"
                                : "show"
                            }`}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Widget Scheduling EmailIds
                            </label>{" "}
                            <div className="custom_emailboxholder">
                              <ul>
                                {this.state.widgetSchedulerEmailIDs.length
                                  ? this.state.widgetSchedulerEmailIDs
                                      .split(";")
                                      .map((_email, _in) => {
                                        if (_email.length)
                                          return (
                                            <li>
                                              {_email}{" "}
                                              <span
                                                onClick={() =>
                                                  this.delEmail(
                                                    _email,
                                                    "widgetSchedulerEmailIDs"
                                                  )
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
                                  this.state.widgetSchedulerEmailIDsError
                                    ? "error"
                                    : ""
                                }`}
                                placeholder="Widget Scheduling EmailIds"
                                name="widgetSchedulerEmailIDsDummy"
                                value={this.state.widgetSchedulerEmailIDsDummy}
                                onChange={(e) => this.handleChange(e)}
                                autocomplete="nope"
                                onBlur={(e) =>
                                  this.bindEnteredEmail(
                                    e,
                                    "widgetSchedulerEmailIDsDummy"
                                  )
                                }
                              />{" "}
                            </div>
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Widget Email Subject
                            </label>{" "}
                            <input
                              type="text"
                              className={`input w-full border mt-2 flex-1 `}
                              name="widgetEmailSubject"
                              placeholder="Widget Email Subject"
                              value={this.state.widgetEmailSubject}
                              onChange={(e) => this.handleChange(e)}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Widget Email Body
                            </label>{" "}
                            <textarea
                              className={`input w-full border mt-2 flex-1 `}
                              placeholder="Email Body"
                              name="widgetEmailBody"
                              value={this.state.widgetEmailBody}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Query
                              <svg
                                xmlns="http://www.w3.org/2000/svg"
                                title="basic tool"
                                width="24"
                                height="24"
                                data-tip
                                data-for="level0tip"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                class="tooltip feather feather-alert-octagon w-6 h-6 mr-2 warningIconPos"
                              >
                                <polygon points="7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"></polygon>
                                <line x1="12" y1="8" x2="12" y2="12"></line>
                                <line x1="12" y1="16" x2="12.01" y2="16"></line>
                              </svg>
                              <ReactTooltip
                                id="level0tip"
                                place="top"
                                effect="solid"
                              >
                                <table>
                                  <thead>
                                    <th>Name</th>
                                    <th>Count</th>
                                  </thead>
                                  <tbody>
                                    <tr>
                                      <td>Unauthorised</td>
                                      <td>3</td>
                                    </tr>
                                    <tr>
                                      <td>Authorised</td>
                                      <td>2</td>
                                    </tr>
                                  </tbody>
                                </table>
                              </ReactTooltip>
                            </label>{" "}
                            <textarea
                              className={`input w-full border mt-2 flex-1 ${
                                this.state.widgetQueryError ? "error" : ""
                              }`}
                              placeholder="Select statement"
                              name="widgetQuery"
                              value={this.state.widgetQuery}
                              onChange={(e) => this.handleChange(e)}
                              required
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level1 Connection String
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="level1ConnectionString"
                              value={this.state.level1ConnectionString}
                              onChange={(e) => this.handleChange(e)}
                            >
                              {this.bindDropdownLookup("connectionStrings")}
                            </select>{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 1 Scheduler Type
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="level1SchedulerType"
                              value={this.state.level1SchedulerType}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="0">Select Scheduler Type</option>
                              {this.bindDropdownLookup("schedulers")}
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 `}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 1 Email Option
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="l1UseGridEmail"
                              value={this.state.l1UseGridEmail}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="true">Grid Users</option>
                              <option value="false">Specific Users</option>
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 ${
                              this.state.l1UseGridEmail === "true"
                                ? "hide"
                                : "show"
                            }`}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 1 Scheduling EmailIds
                            </label>{" "}
                            <div className="custom_emailboxholder">
                              <ul>
                                {this.state.l1SchedulerEmailIDs.length
                                  ? this.state.l1SchedulerEmailIDs
                                      .split(";")
                                      .map((_email, _in) => {
                                        if (_email.length)
                                          return (
                                            <li>
                                              {_email}{" "}
                                              <span
                                                onClick={() =>
                                                  this.delEmail(
                                                    _email,
                                                    "l1SchedulerEmailIDs"
                                                  )
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
                                  this.state.l1SchedulerEmailIDsError
                                    ? "error"
                                    : ""
                                }`}
                                placeholder="Widget Scheduling EmailIds"
                                name="l1SchedulerEmailIDsDummy"
                                value={this.state.l1SchedulerEmailIDsDummy}
                                onChange={(e) => this.handleChange(e)}
                                autocomplete="nope"
                                onBlur={(e) =>
                                  this.bindEnteredEmail(
                                    e,
                                    "l1SchedulerEmailIDsDummy"
                                  )
                                }
                              />{" "}
                            </div>
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level1 Email Subject
                            </label>{" "}
                            <input
                              type="text"
                              className={`input w-full border mt-2 flex-1 `}
                              name="l1EmailSubject"
                              placeholder="Level1 Email Subject"
                              value={this.state.l1EmailSubject}
                              onChange={(e) => this.handleChange(e)}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level1 Email Body
                            </label>{" "}
                            <textarea
                              className={`input w-full border mt-2 flex-1 `}
                              placeholder="Email Body"
                              name="l1EmailBody"
                              value={this.state.l1EmailBody}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 1 Query
                              <svg
                                xmlns="http://www.w3.org/2000/svg"
                                data-tip
                                data-for="level1tip"
                                width="24"
                                height="24"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                class="feather feather-alert-octagon w-6 h-6 mr-2 warningIconPos"
                              >
                                <polygon points="7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"></polygon>
                                <line x1="12" y1="8" x2="12" y2="12"></line>
                                <line x1="12" y1="16" x2="12.01" y2="16"></line>
                              </svg>
                              <ReactTooltip
                                id="level1tip"
                                place="top"
                                effect="solid"
                              >
                                Place holder format should be @ColumnName@ in
                                where condition -
                              </ReactTooltip>
                            </label>{" "}
                            <textarea
                              class="input w-full border mt-2 flex-1"
                              placeholder="Select statement"
                              name="dashbaordQueryL1"
                              value={this.state.dashbaordQueryL1}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                        </div>{" "}
                      </div>
                      <div class="intro-y col-span-12 lg:col-span-6">
                        <div class="intro-y">
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level2 Connection String
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="level2ConnectionString"
                              value={this.state.level2ConnectionString}
                              onChange={(e) => this.handleChange(e)}
                            >
                              {this.bindDropdownLookup("connectionStrings")}
                            </select>{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 2 Scheduler Type
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="level2SchedulerType"
                              value={this.state.level2SchedulerType}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="0">Select Scheduler Type</option>
                              {this.bindDropdownLookup("schedulers")}
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 `}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 2 Email Option
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="l2UseGridEmail"
                              value={this.state.l2UseGridEmail}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="true">Grid Users</option>
                              <option value="false">Specific Users</option>
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 ${
                              this.state.l2UseGridEmail === "true"
                                ? "hide"
                                : "show"
                            }`}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 2 Scheduling EmailIds
                            </label>{" "}
                            <div className="custom_emailboxholder">
                              <ul>
                                {this.state.l2SchedulerEmailIDs.length
                                  ? this.state.l2SchedulerEmailIDs
                                      .split(";")
                                      .map((_email, _in) => {
                                        if (_email.length)
                                          return (
                                            <li>
                                              {_email}{" "}
                                              <span
                                                onClick={() =>
                                                  this.delEmail(
                                                    _email,
                                                    "l2SchedulerEmailIDs"
                                                  )
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
                                  this.state.l2SchedulerEmailIDsError
                                    ? "error"
                                    : ""
                                }`}
                                placeholder="Widget Scheduling EmailIds"
                                name="l2SchedulerEmailIDsDummy"
                                value={this.state.l2SchedulerEmailIDsDummy}
                                onChange={(e) => this.handleChange(e)}
                                autocomplete="nope"
                                onBlur={(e) =>
                                  this.bindEnteredEmail(
                                    e,
                                    "l2SchedulerEmailIDsDummy"
                                  )
                                }
                              />{" "}
                            </div>
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level2 Email Subject
                            </label>{" "}
                            <input
                              type="text"
                              className={`input w-full border mt-2 flex-1 `}
                              name="l2EmailSubject"
                              placeholder="Level2 Email Subject"
                              value={this.state.l2EmailSubject}
                              onChange={(e) => this.handleChange(e)}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level2 Email Body
                            </label>{" "}
                            <textarea
                              className={`input w-full border mt-2 flex-1 `}
                              placeholder="Email Body"
                              name="l2EmailBody"
                              value={this.state.l2EmailBody}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 2 Query
                              <svg
                                xmlns="http://www.w3.org/2000/svg"
                                width="24"
                                height="24"
                                data-tip
                                data-for="level2tip"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                class="feather feather-alert-octagon w-6 h-6 mr-2 warningIconPos"
                              >
                                <polygon points="7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"></polygon>
                                <line x1="12" y1="8" x2="12" y2="12"></line>
                                <line x1="12" y1="16" x2="12.01" y2="16"></line>
                              </svg>
                              <ReactTooltip
                                id="level2tip"
                                place="top"
                                effect="solid"
                              >
                                Place holder format should be @ColumnName@ in
                                where condition -
                              </ReactTooltip>
                            </label>{" "}
                            <textarea
                              class="input w-full border mt-2 flex-1"
                              placeholder="Select statement"
                              name="dashbaordQueryL2"
                              value={this.state.dashbaordQueryL2}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level3 Connection String
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="level3ConnectionString"
                              value={this.state.level3ConnectionString}
                              onChange={(e) => this.handleChange(e)}
                            >
                              {this.bindDropdownLookup("connectionStrings")}
                            </select>{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 3 Scheduler Type
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="level3SchedulerType"
                              value={this.state.level3SchedulerType}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="0">Select Scheduler Type</option>
                              {this.bindDropdownLookup("schedulers")}
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 `}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 3 Email Option
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="l3UseGridEmail"
                              value={this.state.l3UseGridEmail}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="true">Grid Users</option>
                              <option value="false">Specific Users</option>
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 ${
                              this.state.l3UseGridEmail === "true"
                                ? "hide"
                                : "show"
                            }`}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 3 Scheduling EmailIds
                            </label>{" "}
                            <div className="custom_emailboxholder">
                              <ul>
                                {this.state.l3SchedulerEmailIDs.length
                                  ? this.state.l3SchedulerEmailIDs
                                      .split(";")
                                      .map((_email, _in) => {
                                        if (_email.length)
                                          return (
                                            <li>
                                              {_email}{" "}
                                              <span
                                                onClick={() =>
                                                  this.delEmail(
                                                    _email,
                                                    "l3SchedulerEmailIDs"
                                                  )
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
                                  this.state.l3SchedulerEmailIDsError
                                    ? "error"
                                    : ""
                                }`}
                                placeholder="Widget Scheduling EmailIds"
                                name="l3SchedulerEmailIDsDummy"
                                value={this.state.l3SchedulerEmailIDsDummy}
                                onChange={(e) => this.handleChange(e)}
                                autocomplete="nope"
                                onBlur={(e) =>
                                  this.bindEnteredEmail(
                                    e,
                                    "l3SchedulerEmailIDsDummy"
                                  )
                                }
                              />{" "}
                            </div>
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level3 Email Subject
                            </label>{" "}
                            <input
                              type="text"
                              className={`input w-full border mt-2 flex-1 `}
                              name="l3EmailSubject"
                              placeholder="Level3 Email Subject"
                              value={this.state.l3EmailSubject}
                              onChange={(e) => this.handleChange(e)}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level3 Email Body
                            </label>{" "}
                            <textarea
                              className={`input w-full border mt-2 flex-1 `}
                              placeholder="Email Body"
                              name="l3EmailBody"
                              value={this.state.l3EmailBody}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 3 Query
                              <svg
                                xmlns="http://www.w3.org/2000/svg"
                                width="24"
                                height="24"
                                data-tip
                                data-for="level3tip"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                class="feather feather-alert-octagon w-6 h-6 mr-2 warningIconPos"
                              >
                                <polygon points="7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"></polygon>
                                <line x1="12" y1="8" x2="12" y2="12"></line>
                                <line x1="12" y1="16" x2="12.01" y2="16"></line>
                              </svg>
                              <ReactTooltip
                                id="level3tip"
                                place="top"
                                effect="solid"
                              >
                                Place holder format should be @ColumnName@ in
                                where condition -
                              </ReactTooltip>
                            </label>{" "}
                            <textarea
                              class="input w-full border mt-2 flex-1"
                              placeholder="Select statement"
                              name="dashbaordQueryL3"
                              value={this.state.dashbaordQueryL3}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level4 Connection String
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="level4ConnectionString"
                              value={this.state.level4ConnectionString}
                              onChange={(e) => this.handleChange(e)}
                            >
                              {this.bindDropdownLookup("connectionStrings")}
                            </select>{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center mt-3">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 4 Scheduler Type
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="level4SchedulerType"
                              value={this.state.level4SchedulerType}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="0">Select Scheduler Type</option>
                              {this.bindDropdownLookup("schedulers")}
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 `}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 4 Email Option
                            </label>{" "}
                            <select
                              class="select2 w-full input w-full border mt-2 flex-1"
                              name="l4UseGridEmail"
                              value={this.state.l4UseGridEmail}
                              onChange={(e) => this.handleChange(e)}
                            >
                              <option value="true">Grid Users</option>
                              <option value="false">Specific Users</option>
                            </select>{" "}
                          </div>{" "}
                          <div
                            className={`flex flex-col sm:flex-row items-center mt-3 ${
                              this.state.l4UseGridEmail === "true"
                                ? "hide"
                                : "show"
                            }`}
                          >
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 4 Scheduling EmailIds
                            </label>{" "}
                            <div className="custom_emailboxholder">
                              <ul>
                                {this.state.l4SchedulerEmailIDs.length
                                  ? this.state.l4SchedulerEmailIDs
                                      .split(";")
                                      .map((_email, _in) => {
                                        if (_email.length)
                                          return (
                                            <li>
                                              {_email}{" "}
                                              <span
                                                onClick={() =>
                                                  this.delEmail(
                                                    _email,
                                                    "l4SchedulerEmailIDs"
                                                  )
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
                                  this.state.l4SchedulerEmailIDsError
                                    ? "error"
                                    : ""
                                }`}
                                placeholder="Widget Scheduling EmailIds"
                                name="l4SchedulerEmailIDsDummy"
                                autocomplete="nope"
                                value={this.state.l4SchedulerEmailIDsDummy}
                                onChange={(e) => this.handleChange(e)}
                                autocomplete="nope"
                                onBlur={(e) =>
                                  this.bindEnteredEmail(
                                    e,
                                    "l4SchedulerEmailIDsDummy"
                                  )
                                }
                              />{" "}
                            </div>
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level4 Email Subject
                            </label>{" "}
                            <input
                              type="text"
                              className={`input w-full border mt-2 flex-1 `}
                              name="l4EmailSubject"
                              placeholder="Level4 Email Subject"
                              value={this.state.l4EmailSubject}
                              onChange={(e) => this.handleChange(e)}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level4 Email Body
                            </label>{" "}
                            <textarea
                              className={`input w-full border mt-2 flex-1 `}
                              placeholder="Email Body"
                              name="l4EmailBody"
                              value={this.state.l4EmailBody}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div class="flex flex-col sm:flex-row items-center">
                            {" "}
                            <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                              Level 4 Query
                              <svg
                                xmlns="http://www.w3.org/2000/svg"
                                width="24"
                                height="24"
                                data-tip
                                data-for="level4tip"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                class="feather feather-alert-octagon w-6 h-6 mr-2 warningIconPos"
                              >
                                <polygon points="7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"></polygon>
                                <line x1="12" y1="8" x2="12" y2="12"></line>
                                <line x1="12" y1="16" x2="12.01" y2="16"></line>
                              </svg>
                              <ReactTooltip
                                id="level4tip"
                                place="top"
                                effect="solid"
                              >
                                Place holder format should be @ColumnName@ in
                                where condition -
                              </ReactTooltip>
                            </label>{" "}
                            <textarea
                              class="input w-full border mt-2 flex-1"
                              placeholder="Select statement"
                              name="dashbaordQueryL4"
                              value={this.state.dashbaordQueryL4}
                              onChange={(e) => this.handleChange(e)}
                              style={{ height: "100px" }}
                            />{" "}
                          </div>{" "}
                          <div
                            class="sm:ml-20 sm:pl-5 mt-5"
                            style={{ padding: "20px 0" }}
                          >
                            {" "}
                            <button
                              type="button"
                              class="button bg-theme-1 text-white"
                              style={{ float: "right" }}
                              onClick={() => this.addWidget()}
                            >
                              {this.state.widgetId == -1 ? "Add" : "Update"}
                            </button>{" "}
                            <button
                              type="button"
                              class="button bg-theme-1 text-white"
                              style={{ float: "right", marginRight: "10px" }}
                              onClick={() => this.showAddWidget(false, {})}
                            >
                              Cancel
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

          <div
            className={`modal ${
              this.state.delete_modal ? "show model_show" : ""
            }`}
            id="delete-modal-preview"
          >
            {" "}
            <div class="modal__content">
              {" "}
              <div class="p-5 text-center">
                {" "}
                <i
                  data-feather="x-circle"
                  class="w-16 h-16 text-theme-6 mx-auto mt-3"
                ></i>{" "}
                <div class="text-3xl mt-5">Are you sure?</div>{" "}
                <div class="text-gray-600 mt-2">
                  Do you really want to delete the widgets? This process cannot
                  be undone.
                </div>{" "}
              </div>{" "}
              <div class="px-5 pb-8 text-center">
                {" "}
                <button
                  type="button"
                  data-dismiss="modal"
                  class="button w-24 border text-gray-700 mr-1"
                  onClick={() => this.showDeletePop(false, {})}
                >
                  Cancel
                </button>{" "}
                <button
                  type="button"
                  class="button w-24 bg-theme-6 text-white"
                  onClick={() => this.deleteWidget()}
                >
                  Delete
                </button>{" "}
              </div>{" "}
            </div>{" "}
          </div>
        </div>
        <Notification></Notification>
      </>
    );
  }
}
const mapProperties = (state) => {
  return {
    widget_data: state.dashboardReducer.widgetData,
    loginstatus: state.dashboardReducer.loginstatus,
    dashboardDropdown: state.dashboardReducer.dashboardDropdown,
    singleDashboardWidget: state.dashboardReducer.singleDashboardWidget,
  };
};
const dispatch_action = (dispatch) => {
  //console.log("userDetails.UserContext.firmId", userDetails.UserContext.firmId)

  return {
    GET_DASHBOARD_WIDGETS: () => dispatch(action_type._getDashboardWidgets()),
    POST_DASHBOARD_WIDGETS: (_state) =>
      dispatch(action_type._post_dashboardWidget(_state)),
    DELETE_DASHBOARD_WIDGETS: (_id) =>
      dispatch(action_type._delete_dashboardWidget(_id)),
    USER_LOGOUT: () => dispatch(action_type._user_logout()),
    SET_NOTIFICATION: () => dispatch(notify_action_type._setNotify()),
    GET_ALL_DROPDOWNS: () => dispatch(action_type._getDashboardDropdowns()),
    EDIT_DASHBOARD_WIDGET: (_id) =>
      dispatch(action_type._edit_dashboardWidget(_id)),
  };
};

export default connect(mapProperties, dispatch_action)(Dashboard);
