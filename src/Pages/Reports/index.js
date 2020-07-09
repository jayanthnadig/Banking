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
    };
  }
  componentDidMount() {
    let _params = window.location.pathname.split("/");
    let _obj = new Object();
    _obj.clickLevel = "L" + _params[_params.length - 4];
    _obj.clickedWidgetId = parseInt(_params[_params.length - 3]);
    _obj.clickedOnValue = _params[_params.length - 2];
    _obj.gridColumns = [];
    _obj.gridInput = _params[_params.length - 1] === "null" ? [] : JSON.parse(window.atob(_params[_params.length - 1]));
    _obj.gridData = [];
    this.setState({
      header: _obj.clickedOnValue,
    });
    this.props.GET_DASHBOARD_DRILLDOWN(_obj);
  }

  componentWillUnmount() {}
  componentWillReceiveProps(nextProps) {
    console.log(nextProps);
  }
  drillDown(_data) {
    var _list = [];
    this.props.drilldown_data[0].gridColumns.map((xx, ii) => {
      var _obj = new Object();
      _obj.name = xx;
      _obj.value = _data._head[ii];
      _list.push(_obj);
    });

    let _params = window.location.pathname.split("/");
    let _obj = new Object();
    _obj.clickLevel = parseInt(_params[_params.length - 4]) + 1;
    _obj.clickedWidgetId = parseInt(_params[_params.length - 3]);
    _obj.clickedOnValue = _params[_params.length - 2];
  
    window.open(
      `/report/${_obj.clickLevel}/${_obj.clickedWidgetId}/${_obj.clickedOnValue}/${window.btoa(JSON.stringify(
        _list
      ))}`,
      "_blank"
    );
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
          <tr onClick={() => this.drillDown({ _head })}>
            {_head.map((_body) => {
              return <td class="border">{_body}</td>;
            })}
          </tr>
        );
      });
    }
  }
  logout() {
    window.location.href = "/";
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
                      {this.state.header} Transactions
                    </h1>
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
                              {this.bindTableHeader()}
                            </tr>{" "}
                          </thead>{" "}
                          <tbody> {this.bindTableBody()}</tbody>{" "}
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
  };
};

export default connect(mapProperties, dispatch_action)(Report);
