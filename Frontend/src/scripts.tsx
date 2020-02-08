import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import store from './redux/store';

// import App from "./components/App";
import RouterComponent from './components/RouterComponent';

const app = document.getElementById('app');
ReactDOM.render(
  <Provider store={store}>
    <RouterComponent />
  </Provider>, app,
);
