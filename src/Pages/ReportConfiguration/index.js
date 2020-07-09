import React from "react";
import { connect } from "react-redux";
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import logo from "../../dist/images/logo.png";
import * as action_type from "../../actions/reportconfig/reportManagment";

class ReportConfiguration extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
      reportQuery: "",
      reportEmail: "",
      reportFormat: "Excel",
      reportInterval: "1 Hour",
      reportId: -1,
      reportName: "",
      report_dbStatus: false,
    };
  }
  componentDidMount() {
    this.props.GET_REPORT_NAMES();
  }

  componentWillUnmount() {}
  componentWillReceiveProps(nextProps) {
    console.log(nextProps);
    if (nextProps.report_dbStatus) {
      this.clear();
    }
    if(Object.keys(nextProps.report_object).length){
      this.setState({
        reportQuery: nextProps.report_object.reportQuery,
        reportEmail: nextProps.report_object.reportEmail,
        reportFormat: nextProps.report_object.reportFormat,
        reportInterval: nextProps.report_object.reportInterval,
        reportId: nextProps.report_object.reportId,
        reportName: nextProps.report_object.reportName,
      })
    }
  }
  showAddWidget(flag) {
    this.setState((prevState) => ({
      // prevState?
      modal: flag,
    }));
    if (!flag) this.props.GET_REPORT_NAMES();
  }
  logout() {
    window.location.href = "/";
  }
  clear() {
    this.setState({
      reportQuery: "",
      reportEmail: "",
      reportFormat: "Excel",
      reportInterval: "1 Hour",
      reportId: -1,
      reportName: "",
    });
  }
  handleChange = (e) => {
    let _name = e.target.name;
    let _value = e.target.value;
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField[_name] = _value;
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
  addReport() {
    this.props.POST_REPORT_DETAILS(this.state);
    this.clear();
    this.setState({
      report_dbStatus: true,
    });
    setTimeout(() => {
      this.setState({
        report_dbStatus: false,
      });
    }, 2000);
  }
  loadReportDetails(e) {
    this.setState({
      reportId: parseInt(e.target.value),
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
    this.showAddWidget(true);
    this.props.EDIT_REPORT_DETAILS(this.state.reportId)
  }
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

                <li>
                  <a href="/userconfig" class="side-menu">
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
                <li>
                  <a
                    href="#"
                    onClick={() => this.showAddWidget(false)}
                    className={`top-menu ${
                      !this.state.modal ? "top-menu--active" : ""
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
                <li>
                  <a
                    href="#"
                    onClick={() => this.showAddWidget(true)}
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
                        View Reports
                      </h1>
                    </div>
                    <div class="p-5" id="vertical-bar-chart">
                      <div
                        class="flex flex-col sm:flex-row items-center mt-3"
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
                          <option value="0">
                            Select Report to View or Edit
                          </option>
                          {this.loadReportName()}
                        </select>{" "}
                        <button
                          style={{ padding: "0 35px" }}
                          onClick={() => this.editReport()}
                        >
                          Edit
                        </button>
                        <button>Download </button>
                      </div>
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
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div
                className={`intro-y grid grid-cols-12 ${
                  !this.state.modal ? "hidden" : ""
                }`}
              >
                <div class="col-span-12 lg:col-span-12">
                  <div class="intro-y box">
                    <div class="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                      <h1 class="font-medium text-base mr-auto text-4xl ">
                        Config Reports
                      </h1>
                    </div>
                    <div class="p-5" id="vertical-bar-chart">
                      <div class="preview validate-form">
                        <div class="overflow-x-auto">
                          {" "}
                          <div class="modal__content relative custom_model_content">
                            <div
                              className={`rounded-md px-5 py-4 mb-2 bg-theme-9 text-white ${
                                !this.state.report_dbStatus ? "hidden" : ""
                              }`}
                            >
                              Report added successfully{" "}
                            </div>
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Name
                              </label>{" "}
                              <input
                                type="text"
                                class="input w-full border mt-2 flex-1"
                                placeholder="Store Performance"
                                name="reportName"
                                value={this.state.reportName}
                                onChange={(e) => this.handleChange(e)}
                              />{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Query
                              </label>{" "}
                              <textarea
                                class="input w-full border mt-2 flex-1"
                                placeholder="Select statement"
                                style={{ height: "300px" }}
                                name="reportQuery"
                                value={this.state.reportQuery}
                                onChange={(e) => this.handleChange(e)}
                              />{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Email
                              </label>{" "}
                              <input
                                type="text"
                                class="input w-full border mt-2 flex-1"
                                placeholder="Store Performance"
                                name="reportEmail"
                                value={this.state.reportEmail}
                                onChange={(e) => this.handleChange(e)}
                              />{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center mt-3">
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
                            <div class="flex flex-col sm:flex-row items-center mt-3">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Send Report Every
                              </label>{" "}
                              <select
                                class="select2 w-full input w-full border mt-2 flex-1"
                                name="reportInterval"
                                value={this.state.reportInterval}
                                onChange={(e) => this.handleChange(e)}
                              >
                                <option value="1">1 Hour</option>
                                <option value="2">4 Hours</option>
                                <option value="3">6 Hours</option>
                                <option value="3">Once per day</option>
                              </select>{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center mt-3">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                IsActive
                              </label>{" "}
                              <input
                                type="checkbox"
                                checked={true}
                                name="isActive"
                                class="input border mr-2"
                                id="vertical-remember-me"
                              />
                            </div>{" "}
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
                              >
                                Cancel
                              </button>{" "}
                            </div>
                          </div>{" "}
                        </div>
                      </div>
                      <div class="source-code hidden">
                        <button
                          data-target="#copy-vertical-bar-chart"
                          class="copy-code button button--sm border flex items-center text-gray-700"
                        >
                          {" "}
                          <i data-feather="file" class="w-4 h-4 mr-2"></i> Copy
                          code{" "}
                        </button>
                        <div class="overflow-y-auto h-64 mt-3">
                          <pre
                            class="source-preview"
                            id="copy-vertical-bar-chart"
                          >
                            {" "}
                            <code class="text-xs p-0 rounded-md html pl-5 pt-8 pb-4 -mb-10 -mt-10">
                              {" "}
                              HTMLOpenTagcanvas id="vertical-bar-chart-widget"
                              height="200"HTMLCloseTagHTMLOpenTag/canvasHTMLCloseTag{" "}
                            </code>{" "}
                          </pre>
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
    report_name: state.reportReducer.report_name,
    report_dbStatus: state.reportReducer.report_dbStatus,
    report_details: state.reportReducer.report_details,
    report_object: state.reportReducer.report_object,
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
    /*POST_DASHBOARD_WIDGETS: (_state) =>
    
      dispatch(action_type._post_dashboardWidget(_state)),
    DELETE_DASHBOARD_WIDGETS: (_id) =>
      dispatch(action_type._delete_dashboardWidget(_id)),*/
  };
};

export default connect(mapProperties, dispatch_action)(ReportConfiguration);
