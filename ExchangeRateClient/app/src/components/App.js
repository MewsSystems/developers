import React from 'react';
import PropTypes from 'prop-types';
import {connect} from 'react-redux';
import sn from 'classnames';
import s from '../styles/App.less';
import ControlPanel from './ControlPanel';
import Filter from './Filter';
import {getFilteredRates} from '../selectors';


const App = (props) => (
  <div className={sn(s.app)}>
    <ControlPanel />
    <Filter />
    <pre>{JSON.stringify(props.rates, null, ' ')}</pre>
  </div>
);

export default connect(state => ({
  rates: getFilteredRates(state)
}))(App);
