import React from "react";
import ReactDOM from "react-dom";
import Dashboard from './Dashboard';
import { Provider } from "react-redux";
import appStore from './store';
import "core-js/stable";
import "regenerator-runtime/runtime";
import './css/bootstrap.min.css';
import mainStyle from './css/style.css';

class ExchangeRatesApp extends React.Component {
  render() {
    return (
      <Provider store={appStore}>
        <div className="container">
          <Dashboard />
        </div>
      </Provider>
    )
  }
}
ReactDOM.render(<ExchangeRatesApp />, document.getElementById("exchange-rate-client"));
