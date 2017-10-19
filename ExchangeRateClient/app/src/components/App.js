import React from 'react';
import PropTypes from 'prop-types';
import {connect} from 'react-redux';
import sn from 'classnames';
import Filter from './Filter';
import {getFilteredRates} from '../selectors';


const App = (props) => (
  <div>
    <Filter />
    <pre>{JSON.stringify(props.rates, null, ' ')}</pre>
  </div>
);

export default connect(state => ({
  rates: getFilteredRates(state)
}))(App);
