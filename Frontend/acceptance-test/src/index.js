import React from "react";
import ReactDOM from "react-dom";
import { Router } from "react-router";
import { createBrowserHistory } from "history";

import App from "./App";

//Redux
import { Provider } from "react-redux";
import store from "./redux/store";

//Styles
import "./styles/index.scss";

const history = createBrowserHistory();

ReactDOM.render(
  <Provider store={store}>
    <Router history={history}>
      <App />
    </Router>
  </Provider>,
  document.getElementById("root")
);
