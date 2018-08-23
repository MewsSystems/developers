import React from 'react';
import { hot } from 'react-hot-loader';
import { endpoint, interval } from 'Config/app-config.json';

import style from './app.scss';

const App = () => (
  <div className={style.app}>
    Get Rates from {endpoint} at {interval} ms interval
  </div>
);

export default hot(module)(App);
