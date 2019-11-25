import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import { createStore } from "redux";
import { appReducer } from "./store/Reducers";
import { getConfigDTO, getRatesDTO } from "./services/RatesService";
import {
  loadConfigAction,
  setConfigLoaded,
  setFirstRatesLoaded,
  saveRatesAction,
  updateRatesAction,
  togglePairVisibilityAction
} from "./store/Actions";
import { any } from "prop-types";
import { Provider } from "react-redux";
import { store } from "./store/store";
import Loader from "./components/Loader";

ReactDOM.render(
  <Provider store={store}>
    <Loader>
      <App />
    </Loader>
  </Provider>,

  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
