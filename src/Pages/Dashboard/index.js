import React from "react";
import { connect } from "react-redux";
import LookUpUtilities from "../../Common/Utility/LookUpDataMapping";
import * as action_type from "../../actions/dashboard/dashboardManagement"
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import logo from "../../dist/images/logo.png";

class Dashboard extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
      userDetails:{}
    };
  }
  componentDidMount() {
    let _userDetails=LookUpUtilities.LoginDetails();
    this.setState({
        userDetails: _userDetails,
      });
    let chart = am4core.create("chartdiv", am4charts.PieChart);
    let chart1 = am4core.create("chartdiv1", am4charts.XYChart);
    chart.data = [
      {
        country: "Authorized",
        litres: 50,
      },
      {
        country: "UnAuthorized",
        litres: 100,
      },
      
    ];

    chart1.data = [
        {
            country: "Authorized",
            litres: 50,
          },
          {
            country: "UnAuthorized",
            litres: 100,
          },
    ];
   
    this.chart = chart;
    this.chart1 = chart1;
    // Add and configure Series
    let pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "litres";
    pieSeries.dataFields.category = "country";

    let categoryAxis = chart1.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "country";
    categoryAxis.title.text = "Countries";

    let valueAxis = chart1.yAxes.push(new am4charts.ValueAxis());
    valueAxis.title.text = "Litres sold (M)";

    let series = chart1.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = "litres";
    series.dataFields.categoryX = "country";

    pieSeries.slices.template.events.on("hit", function(ev) {
        window.open(
            '/report',
            '_blank' // <- This is what makes it open in a new window.
          );
      }, this);

      series.columns.template.events.on("hit", function(ev) {
        window.open(
            '/report',
            '_blank' // <- This is what makes it open in a new window.
          );
      }, this);
  }

  componentWillUnmount() {
    if (this.chart) {
      this.chart.dispose();
    }
    if (this.chart1) {
      this.chart1.dispose();
    }
  }
  showAddWidget(_flag) {
    this.setState({
      modal: _flag,
    });
  }
  logout(){
    window.location.href="/";
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
                <span className="text-white text-lg ml-3">
                  {" "}
                
                </span>
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
              <div class="intro-x dropdown relative mr-4 sm:mr-6" style={{width:"100%"}}>
                  
                  <span className="custom_logout">Logout</span>
              <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#000" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" className="feather feather-log-out mx-auto custom_logout" onClick={()=>this.logout()}><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path><polyline points="16 17 21 12 16 7"></polyline><line x1="21" y1="12" x2="9" y2="12"></line></svg>
            <span className="userwelcome">Welcome {this.state.userDetails.username}</span>
              </div>
            </div>
          </div>
          <div class="flex">
          <nav class="side-nav">
               
               
                <ul>
                    <li>
                        <a href="/dashboard" class="side-menu side-menu--active">
                            <div class="side-menu__icon"> <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" class="feather feather-home"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path><polyline points="9 22 9 12 15 12 15 22"></polyline></svg> </div>
                            <div class="side-menu__title"> Dashboard </div>
                        </a>
                    </li>
                    
                    <li>
                        <a href="/reportconfig" class="side-menu">
                            <div class="side-menu__icon"> <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" class="feather feather-inbox"><polyline points="22 12 16 12 14 15 10 15 8 12 2 12"></polyline><path d="M5.45 5.11L2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"></path></svg> </div>
                            <div class="side-menu__title"> Reports <i data-feather="chevron-down" class="menu__sub-icon"></i>  </div>
                        </a>

                    </li>

                    <li>
                        <a href="#" class="side-menu">
                            <div class="side-menu__icon"> <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" class="feather feather-mail mx-auto"><path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path><polyline points="22,6 12,13 2,6"></polyline></svg> </div>
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
                  onClick={() => this.showAddWidget(true)}
                >
                  <circle cx="12" cy="12" r="10"></circle>
                  <line x1="12" y1="8" x2="12" y2="16"></line>
                  <line x1="8" y1="12" x2="16" y2="12"></line>
                </svg>

                
                </h2>
                <span className="custom-refresh-text">Refresh</span>
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" class="feather feather-refresh-cw custom-mainrefresh"><polyline points="23 4 23 10 17 10"></polyline><polyline points="1 20 1 14 7 14"></polyline><path d="M3.51 9a9 9 0 0 1 14.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0 0 20.49 15"></path></svg>
            </div>
            <div className="intro-y grid grid-cols-12 gap-6 mt-5">
              <div className="col-span-12 lg:col-span-6">
                <div className="intro-y box">
                  <div className="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200">
                    <h2 className="font-medium text-base mr-auto custom-h2width text-2xl">
                      Pie Chart
                    </h2>
                    <div className="range-holder">
                   
                    <select data-hide-search="true" className="select2 input w-full border mt-2 flex-1 custom-select2">
                            <option value="1">2020</option>
                            <option value="2">Johnny Deep</option>
                            <option value="3">Robert Downey, Jr</option>
                            <option value="4">Samuel L. Jackson</option>
                            <option value="5">Morgan Freeman</option>
                    </select>
                    <select data-hide-search="true" className="select2 input w-full border mt-2 flex-1 custom-select2">
                      
                      <option value="1">Jan</option>
                      <option value="2">Johnny Deep</option>
                      <option value="3">Robert Downey, Jr</option>
                      <option value="4">Samuel L. Jackson</option>
                      <option value="5">Morgan Freeman</option>
              </select>
                    </div>
 
                    <div className="w-full sm:w-auto flex items-center sm:ml-auto mt-3 sm:mt-0">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" className="feather feather-edit mx-auto custom_refresh"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path></svg>
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" class="feather feather-trash-2 mx-auto"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path><line x1="10" y1="11" x2="10" y2="17"></line><line x1="14" y1="11" x2="14" y2="17"></line></svg>
                    </div>
                  </div>
                  <div className="p-5" id="vertical-bar-chart">
                    <div className="preview">
                      <div
                        id="chartdiv"
                        style={{ width: "100%", height: "500px" }}
                      ></div>
                    </div>
                    <div className="source-code hidden">
                      <button
                        data-target="#copy-vertical-bar-chart"
                        className="copy-code button button--sm border flex items-center text-gray-700"
                      >
                        {" "}
                        <i
                          data-feather="file"
                          className="w-4 h-4 mr-2"
                        ></i>{" "}
                        Copy code{" "}
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
              <div className="col-span-12 lg:col-span-6">
                <div className="intro-y box">
                  <div className="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200">
                    <h2 className="font-medium text-base mr-auto custom-h2width text-2xl">
                      Column Chart
                    </h2>
                    <div className="range-holder">
                   
                   <select data-hide-search="true" className="select2 input w-full border mt-2 flex-1 custom-select2">
                           <option value="1">2020</option>
                           <option value="2">Johnny Deep</option>
                           <option value="3">Robert Downey, Jr</option>
                           <option value="4">Samuel L. Jackson</option>
                           <option value="5">Morgan Freeman</option>
                   </select>
                   <select data-hide-search="true" className="select2 input w-full border mt-2 flex-1 custom-select2">
                     
                     <option value="1">Jan</option>
                     <option value="2">Johnny Deep</option>
                     <option value="3">Robert Downey, Jr</option>
                     <option value="4">Samuel L. Jackson</option>
                     <option value="5">Morgan Freeman</option>
             </select>
                   </div>
                    <div className="w-full sm:w-auto flex items-center sm:ml-auto mt-3 sm:mt-0">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" className="feather feather-edit mx-auto custom_refresh"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path></svg>
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" class="feather feather-trash-2 mx-auto"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path><line x1="10" y1="11" x2="10" y2="17"></line><line x1="14" y1="11" x2="14" y2="17"></line></svg>
                    </div>
                  </div>
                  <div className="p-5" id="stacked-bar-chart">
                    <div className="preview">
                      <div
                        id="chartdiv1"
                        style={{ width: "100%", height: "500px" }}
                      ></div>
                    </div>
                    <div className="source-code hidden">
                      <button
                        data-target="#copy-stacked-bar-chart"
                        className="copy-code button button--sm border flex items-center text-gray-700"
                      >
                        {" "}
                        <i
                          data-feather="file"
                          className="w-4 h-4 mr-2"
                        ></i>{" "}
                        Copy code{" "}
                      </button>
                      <div className="overflow-y-auto h-64 mt-3">
                        <pre
                          className="source-preview"
                          id="copy-stacked-bar-chart"
                        >
                          {" "}
                          <code className="text-xs p-0 rounded-md html pl-5 pt-8 pb-4 -mb-10 -mt-10">
                            {" "}
                            HTMLOpenTagcanvas
                            id=&quot;stacked-bar-chart-widget&quot;
                            height=&quot;200&quot;HTMLCloseTagHTMLOpenTag/canvasHTMLCloseTag{" "}
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
        <div
          className={`modal p-10 ${this.state.modal ? "show model_show" : ""}`}
          id="button-modal-preview"
        >
          {" "}
          <div class="modal__content relative custom_model_content">
            <div class="flex flex-col sm:flex-row items-center p-5 border-b border-gray-200">
              <h2 class="font-medium text-base mr-auto">Add Widget</h2>
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
                  onClick={()=>this.showAddWidget(false)}
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
                placeholder="Store Performance"
              />{" "}
            </div>{" "}
            <div class="flex flex-col sm:flex-row items-center mt-3">
              {" "}
              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                Widget Type
              </label>{" "}
              <select class="select2 w-full input w-full border mt-2 flex-1">
                <option value="1">Pie Chart</option>
                <option value="2">Column Chart</option>
                <option value="3">Line Chart</option>
              </select>{" "}
            </div>{" "}
            <div class="flex flex-col sm:flex-row items-center">
              {" "}
              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                Query
              </label>{" "}
              <textarea
                class="input w-full border mt-2 flex-1"
                placeholder="Select statement"
                style={{height: "400px" }}
              />{" "}
            </div>{" "}
            <div class="sm:ml-20 sm:pl-5 mt-5">
              {" "}
              <button type="button" class="button bg-theme-1 text-white"  onClick={() => this.showAddWidget(false)}>
                Add
              </button>{" "}
            </div>
          </div>{" "}
        </div>
        </div>
      </>
    );
  }
}
export default Dashboard;
