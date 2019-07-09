import React from 'react';
import { connect } from 'react-redux';
import CurrencyPairsRateList from './CurrencyPairsRateList.jsx';
import { getConfiguration } from "./actions";

class App extends React.Component {
	componentDidMount () {
		this.props.getConfiguration()
	}

	render () {
		console.log(this.props);
		return (
			<div>
				<CurrencyPairsRateList/>
			</div>
		)
	}
}

const mapStateToProps = function () {
	return { }
};

const mapDispatchToProps = (dispatch) => {
	return { getConfiguration: () => dispatch(getConfiguration()) }
};

export default connect(
	mapStateToProps,
	mapDispatchToProps
)(App)
