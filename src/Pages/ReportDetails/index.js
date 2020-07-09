import React from "react";
import { connect } from "react-redux";
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import logo from "../../dist/images/logo.svg";
import * as action_type from "../../actions/reportconfig/reportManagment";

class ReportDetails extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
    };
  }
  componentDidMount() {
   
  }

  componentWillUnmount() {
  
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
const mapProperties = (state) => {
  return {
    report_name: state.reportReducer.report_name,
  };
};
const dispatch_action = (dispatch) => {
  //console.log("userDetails.UserContext.firmId", userDetails.UserContext.firmId)

  return {
    GET_REPORT_NAMES: () => dispatch(action_type._getReportNames()),
    /*POST_DASHBOARD_WIDGETS: (_state) =>
      dispatch(action_type._post_dashboardWidget(_state)),
    DELETE_DASHBOARD_WIDGETS: (_id) =>
      dispatch(action_type._delete_dashboardWidget(_id)),*/
  };
};

export default connect(mapProperties, dispatch_action)(ReportDetails);

