import { combineReducers } from "redux";
import { connectRouter } from "connected-react-router";
import loginReducer from "./users/loginReducer";
import dashboardReducer from "./dashboard/dashboardReducer";
import reportReducer from "./reportConfig/reportReducer";
import { reducer as formReducer } from 'redux-form';
import notifyReducer from 'react-redux-notify';

const createRouteReducer = (history) => combineReducers({
    router: connectRouter(history),
    notifications: notifyReducer,
    form: formReducer,
    loginReducer,
    dashboardReducer,
    reportReducer
});

export default createRouteReducer;