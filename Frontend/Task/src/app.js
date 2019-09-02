import React from 'react';
import ReactDOM from 'react-dom';
// import { Provider } from 'react-redux';
import AppRouter from './routers/AppRouter';
// import configureStore from './store/configureStore';

import 'normalize.css/normalize.css';
import './styles/styles.scss';

// const store = configureStore();

const jsx = (
  <AppRouter />
  // <Provider store={store}>
  // </Provider>
);

ReactDOM.render(jsx, document.getElementById('app'));
