import React from 'react';
import { connect } from 'react-redux';
import CurrencyPairsRateList from './CurrencyPairsRateList.jsx';
import PairsSelector from './PairsSelector.jsx';
import { getConfiguration, getRate, selectCurrency } from "./actions";

class App extends React.Component {
	constructor () {
		super();

		this.state = {
			currencyPairs: []

	}
	}

	componentDidMount () {
		this.props.getConfiguration()
	}

	componentDidUpdate(prevProps) {
		const prevConfiguration = prevProps.configuration;
		const { configuration } = this.props;

		console.log('JSON.stringify(prevConfiguration) !== JSON.stringify(configuration)', JSON.stringify(prevConfiguration) !== JSON.stringify(configuration));
		if (JSON.stringify(prevConfiguration) !== JSON.stringify(configuration)) {
			const currencyPairs = Object.keys(configuration).map((key) => {
				return {key: key, pair: configuration[key]};
			});
			this.setState({
				currencyPairs
			})
		}
	}

	setCurrency = (id) => {
		const { pairsSelector, selectCurrency } = this.props;
		let newPairsSelector = [];

		if (pairsSelector.includes(id)) {
			newPairsSelector = pairsSelector.filter(pairId => pairId !== id);
		} else {
			newPairsSelector = [...pairsSelector, id];
		}

		selectCurrency(newPairsSelector);
	};

	getSelectedCurrency = () => {
		this.props.getRate();
	};

	render () {
		console.log('this.props', this.props);
		console.log('this.state', this.state.currencyPairs);

		return (
			<div>
				<PairsSelector currencyPairs={this.state.currencyPairs} onChange={this.setCurrency} />
				<button onClick={this.getSelectedCurrency}>Get selected pairs</button>
				<CurrencyPairsRateList/>
			</div>
		)
	}
}

const mapStateToProps = function () {
	return { }
};

const mapDispatchToProps = (dispatch) => {
	return {
		getConfiguration: () => dispatch(getConfiguration()),
		getRate: () => dispatch(getRate()),
		selectCurrency: (data) => dispatch(selectCurrency(data))
	}
};

export default connect(
	mapStateToProps,
	mapDispatchToProps
)(App)
