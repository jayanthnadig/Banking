import React from "react";
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import logo from "../../dist/images/logo.svg";

class ReportDetails extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
    };
  }
  componentDidMount() {
    let chart = am4core.create("chartdiv", am4charts.PieChart);
    let chart1 = am4core.create("chartdiv1", am4charts.XYChart);
    chart.data = [
      {
        country: "Lithuania",
        litres: 501.9,
      },
      {
        country: "Czech Republic",
        litres: 301.9,
      },
      {
        country: "Ireland",
        litres: 201.1,
      },
      {
        country: "Germany",
        litres: 165.8,
      },
      {
        country: "Australia",
        litres: 139.9,
      },
      {
        country: "Austria",
        litres: 128.3,
      },
      {
        country: "UK",
        litres: 99,
      },
      {
        country: "Belgium",
        litres: 60,
      },
      {
        country: "The Netherlands",
        litres: 50,
      },
    ];

    chart1.data = [
      {
        country: "Lithuania",
        litres: 501.9,
      },
      {
        country: "Czech Republic",
        litres: 301.9,
      },
      {
        country: "Ireland",
        litres: 201.1,
      },
      {
        country: "Germany",
        litres: 165.8,
      },
      {
        country: "Australia",
        litres: 139.9,
      },
      {
        country: "Austria",
        litres: 128.3,
      },
      {
        country: "UK",
        litres: 99,
      },
      {
        country: "Belgium",
        litres: 60,
      },
      {
        country: "The Netherlands",
        litres: 50,
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
         
         
          <div className="content">
            <div className="intro-y flex items-center mt-8">
              <h2 className="text-lg font-medium mr-auto"> </h2>
            </div>
            <div className="intro-y grid grid-cols-12 ">
              <div className="col-span-12 lg:col-span-12">
                <div className="intro-y box">
                  <div className="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                    <h1 className="font-medium text-base mr-auto text-4xl " style={{color:"#1C3FAA"}}>Detailed View of Unauthorised Transactions</h1>
                   
                  </div>
                  <div className="p-5" id="vertical-bar-chart">
                    <div className="preview">
                      <div class="overflow-x-auto">
                        {" "}
                        <table class="table">
                          {" "}
                          <thead>
                            {" "}
                            <tr class="text-white">
                              {" "}
                              <th class="border border-b-2 whitespace-no-wrap">Branch Name</th>{" "}
                              <th class="border border-b-2 whitespace-no-wrap">Branch Code</th>{" "}
                              <th class="border border-b-2 whitespace-no-wrap">Marker ID</th>{" "}
                              <th class="border border-b-2 whitespace-no-wrap">Assigned To</th>{" "}
                              <th class="border border-b-2 whitespace-no-wrap">Posting Date</th>{" "}
                              <th class="border border-b-2 whitespace-no-wrap">Function ID</th>{" "}
                              <th class="border border-b-2 whitespace-no-wrap">Transaction Status</th>{" "}
                            </tr>{" "}
                          </thead>{" "}
                          <tbody>
                            {" "}
                            <tr>
                              {" "}
                              <td class="border">MTHATHA PLAZA</td>{" "}
                              <td class="border">221</td>{" "}
                              <td class="border">Umthala Plaza</td>{" "}
                              <td class="border">MZIWAKHE Nitanta</td>{" "}
                              <td class="border">16/06/2020</td>{" "}
                              <td class="border">TVCL</td>{" "}
                              <td class="border">IPR</td>{" "}
                            </tr>{" "}
                            <tr>
                              {" "}
                              <td class="border">MTHATHA PLAZA</td>{" "}
                              <td class="border">221</td>{" "}
                              <td class="border">Umthala Plaza</td>{" "}
                              <td class="border">Kungawo</td>{" "}
                              <td class="border">16/06/2020</td>{" "}
                              <td class="border">TVCL</td>{" "}
                              <td class="border">IPR</td>{" "}
                            </tr>{" "}
                            <tr>
                              {" "}
                              <td class="border">MTHATHA PLAZA</td>{" "}
                              <td class="border">221</td>{" "}
                              <td class="border">Umthala Plaza</td>{" "}
                              <td class="border">Kungawo</td>{" "}
                              <td class="border">16/06/2020</td>{" "}
                              <td class="border">TVCL</td>{" "}
                              <td class="border">IPR</td>{" "}
                            </tr>{" "}
                            <tr>
                              {" "}
                              <td class="border">MTHATHA PLAZA</td>{" "}
                              <td class="border">221</td>{" "}
                              <td class="border">Umthala Plaza</td>{" "}
                              <td class="border">Bokamoso</td>{" "}
                              <td class="border">16/06/2020</td>{" "}
                              <td class="border">TVCL</td>{" "}
                              <td class="border">IPR</td>{" "}
                            </tr>{" "}
                            <tr>
                              {" "}
                              <td class="border">MTHATHA PLAZA</td>{" "}
                              <td class="border">221</td>{" "}
                              <td class="border">Umthala Plaza</td>{" "}
                              <td class="border">Rabada</td>{" "}
                              <td class="border">16/06/2020</td>{" "}
                              <td class="border">TVCL</td>{" "}
                              <td class="border">IPR</td>{" "}
                            </tr>{" "}
                          </tbody>{" "}
                        </table>{" "}
                      </div>
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
            </div>
          </div>
        </div>
        <div
          className={`modal p-10 ${this.state.modal ? "show model_show" : ""}`}
          id="button-modal-preview"
        >
          {" "}
          <div class="modal__content relative custom_model_content">
            <div class="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
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
                  onClick={() => this.showAddWidget(false)}
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
              />{" "}
            </div>{" "}
            <div class="sm:ml-20 sm:pl-5 mt-5">
              {" "}
              <button type="button" class="button bg-theme-1 text-white">
                Add
              </button>{" "}
            </div>
          </div>{" "}
        </div>
      </>
    );
  }
}
export default ReportDetails;
