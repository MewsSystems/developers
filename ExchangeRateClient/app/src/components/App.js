import React from 'react';
import PropTypes from 'prop-types';
import {connect} from 'react-redux';
import sn from 'classnames';
import s from '../styles/common.less';
import {getViewRates} from '../selectors';


const App = (props) => (
  <pre>{JSON.stringify(props.rates, null, ' ')}</pre>
);

export default connect(state => ({
  rates: getViewRates(state)
}))(App);
