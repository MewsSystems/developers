import { connect } from 'react-redux';
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import * as api from './api/index';

import List from './components/List/List';

import './app.css';

class App extends Component {
	componentDidMount() {
    api.getConfiguration();
  }

  render() {
		const {
		isLoadingConfiguration, isLoadingRates, rates, status,
			} = this.props;
		return (
  <List
    isLoadingConfiguration={isLoadingConfiguration}
    isLoadingRates={isLoadingRates}
    rates={rates}
    status={status}
		/>
		);
	}
}
const mapStateToProps = (state) => ({
	rates: state.CurrencyReducer.rates,
	isLoadingConfiguration: state.CurrencyReducer.isLoadingConfiguration,
	isLoadingRates: state.CurrencyReducer.isLoadingRates,
	status: state.CurrencyReducer.status,
});

App.propTypes = {
  rates: PropTypes.array,
	isLoadingConfiguration: PropTypes.bool,
	isLoadingRates: PropTypes.bool,
	status: PropTypes.number,
};

App.defaultProps = {
  rates: [],
	status: 200,
	isLoadingConfiguration: true,
	isLoadingRates: true,
};

export default connect(mapStateToProps)(App);
