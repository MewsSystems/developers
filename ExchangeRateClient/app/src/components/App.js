import React from 'react';
import s from '../styles/App.less';
import ControlPanel from './ControlPanel';
import Filter from './Filter';
import Rates from './Rates';
import {getFilteredRates} from '../selectors';


const App = () => (
  <div className={s.app}>
    <ControlPanel />
    <Filter />
    <Rates />
  </div>
);

export default App;
