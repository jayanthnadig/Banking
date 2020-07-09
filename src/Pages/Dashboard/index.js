import React from "react";
import { connect } from "react-redux";
import ReactTooltip from "react-tooltip";
import LookUpUtilities from "../../Common/Utility/LookUpDataMapping";
import * as action_type from "../../actions/dashboard/dashboardManagement";
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import logo from "../../dist/images/logo.png";

class Dashboard extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
      delete_modal: false,
      userDetails: {},
      widgetName: "",
      widgetId: -1,
      widgetType: "Line Chart",
      widgetQuery: "",
      widgetQueryLevel1: "",
      widgetQueryLevel2: "",
      widgetQueryLevel3: "",
    };
  }
  componentDidMount() {
    let _userDetails = LookUpUtilities.LoginDetails();
    this.setState({
      userDetails: _userDetails,
    });
    this.props.GET_DASHBOARD_WIDGETS(_userDetails.userid);
    // let chart = am4core.create("chartdiv", am4charts.PieChart);
    // let chart1 = am4core.create("chartdiv1", am4charts.XYChart);
    // chart.data = [
    //   {
    //     country: "Authorized",
    //     litres: 50,
    //   },
    //   {
    //     country: "UnAuthorized",
    //     litres: 100,
    //   },

    // ];

    // chart1.data = [
    //     {
    //         country: "Authorized",
    //         litres: 50,
    //       },
    //       {
    //         country: "UnAuthorized",
    //         litres: 100,
    //       },
    // ];

    // this.chart = chart;
    // this.chart1 = chart1;

    // let pieSeries = chart.series.push(new am4charts.PieSeries());
    // pieSeries.dataFields.value = "litres";
    // pieSeries.dataFields.category = "country";

    // let categoryAxis = chart1.xAxes.push(new am4charts.CategoryAxis());
    // categoryAxis.dataFields.category = "country";
    // categoryAxis.title.text = "Countries";

    // let valueAxis = chart1.yAxes.push(new am4charts.ValueAxis());
    // valueAxis.title.text = "Litres sold (M)";

    // let series = chart1.series.push(new am4charts.ColumnSeries());
    // series.dataFields.valueY = "litres";
    // series.dataFields.categoryX = "country";

    // pieSeries.slices.template.events.on("hit", function(ev) {
    //     window.open(
    //         '/report',
    //         '_blank'
    //       );
    //   }, this);

    //   series.columns.template.events.on("hit", function(ev) {
    //     window.open(
    //         '/report',
    //         '_blank'
    //       );
    //   }, this);
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
    //  this.bindDatatoChart()
  }
  showAddWidget(_flag, _obj) {
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField.modal = _flag;
      if (Object.keys(_obj).length) {
        _tempField.widgetName = _obj.widgetName;
        _tempField.widgetId = _obj.widgetId;
        _tempField.widgetType = _obj.widgetType;
        _tempField.widgetQuery = _obj.widgetQuery;
        _tempField.widgetQueryLevel1 = _obj.widgetQueryLevel1;
        _tempField.widgetQueryLevel2 = _obj.widgetQueryLevel2;
        _tempField.widgetQueryLevel3 = _obj.widgetQueryLevel3;
      } else {
        _tempField.widgetName = "";
        _tempField.widgetId = -1;
        _tempField.widgetType = "Pie Chart";
        _tempField.widgetQuery = "";
        _tempField.widgetQueryLevel1 = "";
        _tempField.widgetQueryLevel2 = "";
        _tempField.widgetQueryLevel3 = "";
      }
      return _tempField;
    });
  }
  showDeletePop(_flag, _obj) {
    this.setState((prevState) => {
      let _tempField = Object.assign({}, prevState);
      _tempField.delete_modal = _flag;
      if (Object.keys(_obj).length) {
        _tempField.widgetId = _obj.widgetId;
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
    window.location.href = "/";
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
  addWidget = () => {
    console.log(this.state.widgetName);
    this.props.POST_DASHBOARD_WIDGETS(this.state);
  };
  bindDatatoChart() {
    if (this.props.widget_data.length) {
      return this.props.widget_data.map((_chartData, index) => {
        if (_chartData.widgetType == "Pie Chart") {
          let chart = am4core.create(
            "chartdiv" + index + "",
            am4charts.PieChart
          );
          chart.data = _chartData.widgetData;

          this.chart = chart;

          let pieSeries = chart.series.push(new am4charts.PieSeries());
          pieSeries.dataFields.value = "count";
          pieSeries.dataFields.category = "name";

          pieSeries.slices.template.events.on(
            "hit",
            function (ev) {
              //console.log(ev.target.dataItem.dataContext.name);
              window.open(
                `/report/${1}/${_chartData.widgetId}/${ev.target.dataItem.dataContext.name}/null`,
                "_blank"
              );
            },
            this
          );
        } else if (_chartData.widgetType == "Bar Chart") {
          let chart = am4core.create(
            "chartdiv" + index + "",
            am4charts.XYChart
          );
          chart.data = _chartData.widgetData;

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
                `/report/${1}/${_chartData.widgetId}/${ev.target.dataItem.dataContext.name}/null`,
                "_blank"
              );
            },
            this
          );
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
                  {_chartData.widgetName}
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
                    className="feather feather-edit mx-auto custom_refresh"
                    onClick={() => this.showAddWidget(true, _chartData)}
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
                    class="feather feather-trash-2 mx-auto"
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
                <span className="userwelcome">Welcome Admin</span>
              </div>
            </div>
          </div>
          <div class="flex">
            <nav class="side-nav">
              <ul>
                <li>
                  <a href="/dashboard" class="side-menu side-menu--active">
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

                <li>
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
                  Chart{" "}
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
                    className="feather feather-plus-circle mx-auto custom_pluscircle"
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
              <div className="intro-y grid grid-cols-12 gap-6 mt-5">
                {this.bindChartContainer()}
              </div>
            </div>
          </div>
          <div
            className={`modal p-10 ${
              this.state.modal ? "show model_show" : ""
            }`}
            id="button-modal-preview"
          >
            {" "}
            <div class="modal__content relative custom_model_content validate-form">
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
              <div class="flex flex-col sm:flex-row items-center">
                {" "}
                <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                  Name
                </label>{" "}
                <input
                  type="text"
                  class="input w-full border mt-2 flex-1"
                  name="widgetName"
                  placeholder="Widget Name"
                  required
                  value={this.state.widgetName}
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
                  name="widgetType"
                  value={this.state.widgetType}
                  onChange={(e) => this.handleChange(e)}
                >
                  <option value="Pie Chart">Pie Chart</option>
                  <option value="Bar Chart">Bar Chart</option>
                  <option value="Line Chart">Line Chart</option>
                </select>{" "}
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
                  <ReactTooltip id="level0tip" place="top" effect="solid">
                   <table>
                     <thead>
                       <th>Name</th>
                       <th>Count</th>
                     </thead>
                     <tbody>
                       <tr><td>Unauthorised</td><td>3</td></tr>
                       <tr><td>Authorised</td><td>2</td></tr>
                     </tbody>
                   </table>
                  </ReactTooltip>
                </label>{" "}
                <textarea
                  class="input w-full border mt-2 flex-1"
                  placeholder="Select statement"
                  name="widgetQuery"
                  value={this.state.widgetQuery}
                  onChange={(e) => this.handleChange(e)}
                  required
                  style={{ height: "120px" }}
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
                  <ReactTooltip id="level1tip" place="top" effect="solid">
                  Place holder format should be @ColumnName@ in where condition -
                  </ReactTooltip>
                </label>{" "}
                <textarea
                  class="input w-full border mt-2 flex-1"
                  placeholder="Select statement"
                  name="widgetQueryLevel1"
                  value={this.state.widgetQueryLevel1}
                  onChange={(e) => this.handleChange(e)}
                  style={{ height: "120px" }}
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
                  <ReactTooltip id="level2tip" place="top" effect="solid">
                  Place holder format should be @ColumnName@ in where condition -
                  </ReactTooltip>
                </label>{" "}
                <textarea
                  class="input w-full border mt-2 flex-1"
                  placeholder="Select statement"
                  name="widgetQueryLevel2"
                  value={this.state.widgetQueryLevel2}
                  onChange={(e) => this.handleChange(e)}
                  style={{ height: "120px" }}
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
                  <ReactTooltip id="level3tip" place="top" effect="solid">
                  Place holder format should be @ColumnName@ in where condition -
                  </ReactTooltip>
                </label>{" "}
                <textarea
                  class="input w-full border mt-2 flex-1"
                  placeholder="Select statement"
                  name="widgetQueryLevel3"
                  value={this.state.widgetQueryLevel3}
                  onChange={(e) => this.handleChange(e)}
                  style={{ height: "120px" }}
                />{" "}
              </div>{" "}
              <div class="sm:ml-20 sm:pl-5 mt-5" style={{ padding: "20px 0" }}>
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
            </div>{" "}
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
      </>
    );
  }
}
const mapProperties = (state) => {
  return {
    widget_data: state.dashboardReducer.widgetData,
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
  };
};

export default connect(mapProperties, dispatch_action)(Dashboard);
