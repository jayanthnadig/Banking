import React from "react";
import { connect } from "react-redux";
import * as action_type from "../../actions/notification/notifiyaction";
import success_tick from "../../dist/images/check-mark.svg";
import warning_tick from "../../dist/images/warning-mark.svg";

class Notification extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      show: false,
      msg: "",
      flag: -1,
    };
  }
  componentDidMount() {
    console.log("notification", this);
    /* setTimeout(()=>{
        this.setState({
            show:true
        })
    },2000)

    setTimeout(()=>{
        this.setState({
            show:false
        })
    },15000)*/
  }
  componentWillReceiveProps(nextProps) {
    console.log(nextProps);
    this.setState({
      show: nextProps.dashboard_status,
      msg: nextProps.dashboard_msg,
      flag: nextProps.dashboard_status_type,
    });
    if (nextProps.dashboard_status) {
      setTimeout(() => {
        this.props.RESET_NOTIFICATION();
      }, 5000);
    }
  }
  closeNotification() {
    this.props.RESET_NOTIFICATION();
  }
  render() {
    return (
      <>
        <div
          className={`notification notification_pos ${
            this.state.flag === 1
              ? "notification_success"
              : "notification_error "
          } ${this.state.show ? "notify_show" : "notify_hide"}`}
        >
          <img
            alt="Icon"
            src={this.state.flag === 1 ? success_tick : warning_tick}
            className="notify_symbol"
          />
          <h2>{this.state.msg}</h2>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            onClick={() => this.closeNotification()}
            width="20"
            height="20"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="1.5"
            stroke-linecap="round"
            stroke-linejoin="round"
            className="feather feather-x-circle mx-auto notify_close"
          >
            <circle cx="12" cy="12" r="10"></circle>
            <line x1="15" y1="9" x2="9" y2="15"></line>
            <line x1="9" y1="9" x2="15" y2="15"></line>
          </svg>
        </div>
      </>
    );
  }
}
const mapProperties = (state) => {
  return {
    dashboard_status: state.notificationReducer.g_dbStatus,
    dashboard_msg: state.notificationReducer.g_dbMsg,
    dashboard_status_type: state.notificationReducer.g_flag,
  };
};
const dispatch_action = (dispatch) => {
  //console.log("userDetails.UserContext.firmId", userDetails.UserContext.firmId)

  return {
    RESET_NOTIFICATION: () => dispatch(action_type._resetNotify({})),
  };
};
export default connect(mapProperties, dispatch_action)(Notification);
