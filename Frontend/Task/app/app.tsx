import * as React from "react";
import * as ReactDOM from "react-dom";
import { store } from "./config/store";
import { ReduxPersist } from "./store/reduxPersist";
import _throttle from "lodash/throttle";
import { Provider } from "react-redux";
import { Page } from "./components/Page";

store.subscribe(
  _throttle(() => {
    ReduxPersist.saveState({
      rates: store.getState().rates,
      currencyPairs: store.getState().currencyPairs
    });
  }, 1000)
);

const Root: React.SFC<{}> = () => (
  <Provider store={store}>
    <Page />
  </Provider>
);

ReactDOM.render(<Root />, document.getElementById("exchange-rate-client"));
