import React from 'react';
import ReactDom from 'react-dom';
import {Provider} from 'react-redux';
import appStore from './app-store';
import App from './components/App';


ReactDom.render(
  <Provider store={appStore}>
    <App />
  </Provider>,
  document.getElementById('root')
);
