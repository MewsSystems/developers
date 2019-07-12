import React from 'react';
import { connect } from 'react-redux';
import CurrencyPairsRateList from './CurrencyPairsRateList.jsx';
import PairsSelector from './PairsSelector.jsx';
import { getConfiguration, getRate, selectCurrency } from "./actions";

class App extends React.Component {
	getConfig;

	constructor () {
		super();

		this.state = {
			currencyPairs: [],
			rateList: []
		}
	}

	componentDidMount () {
		const { getConfiguration } = this.props;

		let getConfig = setTimeout(function runGetConfig () {
			getConfiguration();
			getConfig = setTimeout(runGetConfig, 30000);
		}, 400);

		this.getConfig = getConfig;
	}

	componentDidUpdate (prevProps) {
		const prevConfiguration = prevProps.configuration;
		const prevCurrencyPairsRateList = prevProps.currencyPairsRateList;
		const { configuration, currencyPairsRateList } = this.props;

		if (JSON.stringify(prevConfiguration) !== JSON.stringify(configuration)) {
			const currencyPairs = Object.keys(configuration).map((key) => {
				return { key: key, pair: configuration[key] };
			});
			this.setState({
				currencyPairs
			})
		}

		if (JSON.stringify(prevCurrencyPairsRateList) !== JSON.stringify(currencyPairsRateList)) {
			const rateList = [];

			currencyPairsRateList.forEach(currentPair => {
				const prevPair = prevCurrencyPairsRateList.find(prevPair => prevPair.pairId === currentPair.pairId);
				let pairRateStatus = 'new choice';

				if (prevPair) {
					const dif = currentPair.rate - prevPair.rate;

					if (!dif) {
						pairRateStatus = 'stagnating';
					} else if (dif > 0) {
						pairRateStatus = 'growing';
					} else if (dif < 0) {
						pairRateStatus = 'declining';
					}
				}

				rateList.push({
					pairId: currentPair.pairId,
					currency: `${currentPair.pairs[0].code}/${currentPair.pairs[1].code}`,
					rate: currentPair.rate,
					trend: pairRateStatus
				});
			});

			this.setState({rateList});
		}
	}

	componentWillUnmount() {
		clearTimeout(this.getConfig);
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
		return (
			<div>
				<PairsSelector currencyPairs={this.state.currencyPairs} onChange={this.setCurrency}/>
				<button onClick={this.getSelectedCurrency}>Get selected pairs</button>
				<CurrencyPairsRateList rateList={this.state.rateList}/>
			</div>
		)
	}
}

const mapStateToProps = function () {
	return {}
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
