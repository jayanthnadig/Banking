import thunk from "redux-thunk";
import { createStore,applyMiddleware,compose } from "redux";
import { createBrowserHistory } from "history";
import { routerMiddleware } from 'connected-react-router';
import  createRouteReducer from "../reducers/rootReducer";
export const history = createBrowserHistory({basename: "/webapp/" });

const storeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
const store = createStore(createRouteReducer(history),storeEnhancers(applyMiddleware(routerMiddleware(history),thunk)));

export default store;