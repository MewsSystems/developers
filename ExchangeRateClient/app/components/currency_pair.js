import React from 'react';
import axios from 'axios';
import { RATE_NEW } from '../reducers/rates.js';
import { endpoint } from '../config.json';
import styles from '../styles.css';

export class CurrencyPair extends React.Component {
	constructor() {
		super()
		this.state = { error: null }
	}
	componentDidMount() {
		const { store } = this.props;
		this.startPoll()
	}
	componentWillUnmount() {
		this.stopPoll() /* that will stop interval timer when currency is off */
	}
	stopPoll() {
		if (this.state && this.state.intervalId) clearInterval(this.state.intervalId);
	}
	startPoll() {
		var intervalId = setInterval( this.getRate.bind(this), 5000);
		this.setState({intervalId: intervalId });
	}
	getRate() {
		const { store, value } = this.props;

		axios.get(endpoint, { params: { currencyPairIds: [value.id] }})
		.then((res) => {
				store.dispatch({type: RATE_NEW, id: value.id, rate: res.data.rates[value.id]});
				this.setState({error: null});
		})
		.catch((error) => {
			this.setState({error: "network issue"})
			console.log(error);
		});
	}
	
	render() {
		const {error} = this.state;
		const {value} = this.props;

		return(<div className={styles.currencyRate}>
				<p className={error ? styles.error : null}>
					{value.pair[0].code} / {value.pair[1].code}
	 				&nbsp; {value.rate ? value.rate : "pending"} 
	 				&nbsp; {value.trend}
	 				&nbsp; {error ? <span>network issue! </span> : null}
	 			</p>
			   </div>)
	}
}