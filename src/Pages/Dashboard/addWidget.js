import React from "react";
import { connect } from "react-redux";
import LookUpUtilities from "../../Common/Utility/LookUpDataMapping";
import * as action_type from "../../actions/dashboard/dashboardManagement";
import logo from "../../dist/images/logo.png";

class addWidget extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
      header: "",
    };
  }
  componentDidMount() {
    
  }

  componentWillUnmount() {}
  componentWillReceiveProps(nextProps) {
    console.log(nextProps);
  }
  
  render() {
    return (
      <>
       
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

export default connect(mapProperties, dispatch_action)(addWidget);
