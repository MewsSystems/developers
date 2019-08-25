import { connect } from 'react-redux';
import React, { Component } from 'react';
import * as api from './api/index';
import {compareRates,get} from './utils/index';

class App extends Component {

	componentDidMount() {
    api.getConfiguration();
  }

  render() {
		console.log(this.props);
		return (
			<div>1</div>
		)
	}
}
const mapStateToProps = state => ({
  configuration: state.currencyReducer.configuration,
	rates: state.currencyReducer.rates,
	isLoadingConfiguration: state.currencyReducer.isLoadingConfiguration,
	isLoadingRates: state.currencyReducer.isLoadingRates,
	status: state.currencyReducer.status,
	request: state.currencyReducer.request,
});

export default connect(mapStateToProps)(App);
