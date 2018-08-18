import React, { Component } from 'react'
import Store from 'store';
import fetch from 'unfetch';
import { toaster } from 'evergreen-ui';
import conf from './config.json';

const ExchangeRate = React.createContext();

class Context extends Component {
	constructor(props) {
		super(props)
		this.url = 'http://localhost:3000';
		this.rateUrl = conf.endpoint;
		this.state = {
			pairs: null,
			interval: conf.interval
		}
	}
	getCurrencyPairs() {
		const pairs = Store.get('pairs');
		if (!pairs) {
			fetch(this.url + '/configuration')
				.then(data => {
					if (!data.ok) {
						// return this.error({ title: 'Server is not Responding', description: 'Server seems down for now try again later' })
						return;
					}
					return data.json();
				})
				.then(data => {
					if (data) {
						const { currencyPairs } = data;
						Store.set('pairs', currencyPairs)
						this.setState({ pairs: currencyPairs })
					}
				})
			return;
		}
		this.setState({ pairs })
	}
	getRate(pairId, cb) {
		const rate = Store.get(`Rate-${pairId}`);
		fetch(this.rateUrl + `?currencyPairIds[]=${pairId}`)
			.then(data => {
				if (!data.ok) {
					return;
				}
				return data.json()
			})
			.then(data => {
				if (data) {
					const { rates } = data;
					Store.set(`Rate-${pairId}`, rates[pairId]);
					this.setState({
						[`Rate-${pairId}`]: rates[pairId]
					})
					return cb(rate, rates[pairId]);
				}
				return cb(rate, rate)
			})
	}
	getLatestRates() {
		const pairs = Store.get('pairs');
		if (pairs) {
			Object.keys(pairs).forEach(pair => {
				const Rate = Store.get(`Rate-${pair}`);
				this.setState({
					[`Rate-${pair}`]: Rate || null
				})
			});
		}
	}
	error({ title, description }) {
		toaster.danger(title, { description })
	}
	componentDidMount() {
		this.getCurrencyPairs();
		this.getLatestRates();
	}
	render() {
		return (
			<ExchangeRate.Provider
				value={{
					...this.state,
					getRate: this.getRate.bind(this)
				}}
			>
				{this.props.children}
			</ExchangeRate.Provider>
		);
	}
}

const withContext = (C) => {
	return class WithContextComponent extends Component {
		render() {
			return (
				<ExchangeRate.Consumer>
					{context => (<C {...this.props} context={context} />)}
				</ExchangeRate.Consumer>
			);
		}
	}
};

export {
	ExchangeRate, Context, withContext
};