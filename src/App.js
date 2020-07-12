import React, { Fragment, Component } from "react";
import { ConnectedRouter } from "connected-react-router";
import { createBrowserHistory } from "history";
import logo from "./logo.svg";
import "./App.css";
import Login from "./Pages/Login";
import Dashboard from "./Pages/Dashboard";
import ReportDetails from "./Pages/ReportDetails";
import Report from "./Pages/Reports";
import ReportConfiguration from "./Pages/ReportConfiguration";
import UserConfig from "./Pages/UserConfig";
import addWidget from "./Pages/Dashboard/addWidget";
//import { history } from "./Store";
import { Switch, Route, browserHistory } from "react-router-dom";

class App extends Component {
  constructor(props) {
    super(props);
    this.history = createBrowserHistory();
  }
  render() {
    return (
      <Fragment>
        <ConnectedRouter history={this.history}>
          <Switch>
            <Route exact path="/" component={() => <Login />} />
            <Route exact path="/dashboard" component={() => <Dashboard />} />
            <Route
              exact
              path="/report/:id/:id/:id/:id"
              component={() => <Report />}
            />
            <Route
              exact
              path="/reportdetails"
              component={() => <ReportDetails />}
            />
            <Route
              exact
              path="/reportconfig"
              component={() => <ReportConfiguration />}
            />
            <Route exact path="/userconfig" component={() => <UserConfig />} />
            <Route exact path="/addwidget" component={() => <addWidget />} />
          </Switch>
        </ConnectedRouter>
      </Fragment>
    );
  }
}

export default App;
