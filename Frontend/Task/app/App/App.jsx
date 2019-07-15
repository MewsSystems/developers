import React from 'react';
import { connect } from 'react-redux';
import PairsRateList from '../PairsRateList/PairsRateList.jsx';
import PairsSelector from '../PairsSelector/PairsSelector.jsx';
import { getConfiguration, getRate, selectCurrency } from "../store/actions";
import { currencyLocalDB } from '../localDb';
import './App.scss';

class App extends React.Component {
	getConfig;

	constructor () {
		super();

		this.state = {
			pairs: [],
			rateList: []
		}
	}

	componentDidMount () {
		const { getConfiguration, configuration, pairsSelector } = this.props;

		this.setConfiguration(configuration, pairsSelector);

		let getConfig = setTimeout(function runGetConfig () {
			getConfiguration();
			getConfig = setTimeout(runGetConfig, 30000);
		}, 400);

		this.getConfig = getConfig;
	}

	componentDidUpdate (prevProps) {
		const prevCurrencyPairsRateList = prevProps.currencyPairsRateList;
		const { currencyPairsRateList } = this.props;

		if (JSON.stringify(prevCurrencyPairsRateList) !== JSON.stringify(currencyPairsRateList)) {
			const rateList = [];

			currencyPairsRateList.forEach(currentPair => {
				const prevPair = prevCurrencyPairsRateList.find(prevPair => prevPair.pairId === currentPair.pairId);
				let pairRateStatus = '';

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

	setConfiguration = (configuration, pairsSelector) => {
		const pairs = Object.keys(configuration).map((key) => {
			return { key: key, pair: configuration[key], selected: pairsSelector.includes(key) };
		});

		this.setState({
			pairs: pairs
		})
	};

	selectPair = (id) => {
		const { pairsSelector, selectCurrency } = this.props;
		const { pairs } = this.state;

		let newPairsSelector = [];
		let newPairs = [...pairs];

		newPairs.map(pair => {
			if (pair.key === id) {
				pair.selected = !pair.selected;
			}
			return pair;
		});

		if (pairsSelector.includes(id)) {
			newPairsSelector = pairsSelector.filter(pairId => pairId !== id);
		} else {
			newPairsSelector = [...pairsSelector, id];
		}

		selectCurrency(newPairsSelector);
		currencyLocalDB.set('currency_user_selection', newPairsSelector);

		this.setState({
			pairs: newPairs
		});

		this.getSelectedCurrencyRate();
	};

	getSelectedCurrencyRate = () => {
		this.props.getRate();
	};

	render () {
		return (
			<div className='container'>
				<PairsSelector pairs={this.state.pairs} onChange={this.selectPair}/>
				<PairsRateList rateList={this.state.rateList}/>
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
