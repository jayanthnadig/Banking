import React from "react";
import { connect } from "react-redux";
import spinner from "../../dist/images/spinner.svg";

class Spinner extends React.Component {
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
  }
  componentWillReceiveProps(nextProps) {
    if (
      nextProps.defaultLoading 
    ) {
      this.setState({ show: true });
    } else {
      this.setState({ show: false });
    }
  }

  render() {
    return (
      <>
        <div className={`spinner_holder ${!this.state.show ? "hide" : ""}`}>
          <img src={spinner} />
        </div>
      </>
    );
  }
}
const mapProperties = (state) => {
  return {
    defaultLoading: state.notificationReducer.loading,
  
  };
};
const dispatch_action = (dispatch) => {
  //console.log("userDetails.UserContext.firmId", userDetails.UserContext.firmId)

  return {};
};
export default connect(mapProperties, dispatch_action)(Spinner);
