import { endpoint, configuration, interval } from './config';
import React from 'react';
import ReactDOM from 'react-dom';
import { createStore } from 'redux';
import { connect, Provider } from 'react-redux';
import { loadState, saveState } from './localStorage';
import { CurrencyFilter } from './components/currency_filter.js'
import { CurrencyPair } from './components/currency_pair.js'
import { currencyRatesStore, CURRENCY_PAIR_NEW } from './reducers/rates.js'
import axios from 'axios';
import styles from './styles.css';

const persistedState = loadState();
const store = createStore(currencyRatesStore, persistedState);


class App extends React.Component {
	constructor() {
		super();
		
		this.state={ pairs: [], message: 'loading ...'};
	}
	componentDidMount() {
		if (this.props.value.length == 0) {
			axios.get(configuration)
			.then((conf) => {
				this.setState({message: 'loaded from network.'});
				for (var item in conf.data.currencyPairs) { 
					store.dispatch({type: CURRENCY_PAIR_NEW, 
						id: item, 
						pair: conf.data.currencyPairs[item],
						selected: true
					});
				 };
			});	
		} else { this.setState({message: 'loaded from cache.'}); }
		
	}
	render() {
		const { value, store } = this.props;
		const { message } = this.state;

		return (<div>
					<header>
						<h2>Currency pairs is {message}</h2>
					</header>
					<div className={styles.app}>
						<div className={styles.sidebar}>
							<button onClick={ () => { saveState(); location.reload();} }>Clear cache and reload from net</button>
							<CurrencyFilter list={value} store={store}/>
						</div>
						<div>
							{value.map((i, index) => { return (i.selected ? <CurrencyPair key={i.id} value={i} index={index} store={store}/>  : null)})}
						</div>
					</div>
				</div>)
	} 
}

export function run() {
	ReactDOM.render(<App value={store.getState()} store={store}/>, document.getElementById("exchange-rate-client"))
}
store.subscribe( () => { saveState(store.getState()) });
store.subscribe(run);
