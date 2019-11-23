import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import { createStore } from "redux";
import { appReducer } from "./store/Reducers";
import { getConfigDTO, getRatesDTP } from "./services/RatesService";
import {
  loadConfigAction,
  setConfigLoaded,
  setFirstRatesLoaded,
  saveRatesAction,
  updateRatesAction,
  togglePairVisibilityAction
} from "./store/Actions";
import { any } from "prop-types";

const store = createStore(appReducer);

console.log(store.getState());
getConfigDTO()
  .then(payload => {
    console.log(payload);
    store.dispatch(loadConfigAction(payload));
    store.dispatch(setConfigLoaded(true));
    console.log(store.getState());
  })
  .then(() => {
    var ids: string[] = Object.keys(store.getState().currencyPairs);
    return getRatesDTP(ids);
  })
  .then(payload => {
    store.dispatch(saveRatesAction(payload));
    store.dispatch(setFirstRatesLoaded(true));
    console.log("saverates action");
    console.log(store.getState());
  })
  .catch(err => console.log(err))
  .then(() => {
    const sleep = (m: number) => new Promise(r => setTimeout(r, m));
    return sleep(10000);
  })
  .then(() => {
    var ids: string[] = Object.keys(store.getState().currencyPairs);
    return getRatesDTP(ids);
  })
  .then(payload => {
    store.dispatch(updateRatesAction(payload));
    console.log(store.getState());
  })
  .then(() => {
    store.dispatch(
      togglePairVisibilityAction(Object.keys(store.getState().currencyPairs)[0])
    );
    console.log(store.getState());
  });

ReactDOM.render(<App />, document.getElementById("root"));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
