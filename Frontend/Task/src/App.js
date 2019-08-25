import { connect } from 'react-redux';
import React, { Component } from 'react';


import * as api from './api/index';

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
});

export default connect(mapStateToProps)(App);
