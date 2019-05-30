import React from 'react';
import { Provider } from 'react-redux';
// import { hot } from 'react-hot-loader/root';

import App from './App';

function RootComponent(props) {
  return (
    <Provider store={props.store}>
      <App />
    </Provider>
  );
}

export default RootComponent;
