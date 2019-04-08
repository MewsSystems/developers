import store, { State, updateRates } from '../store/store';
import { CurrencyPairs } from '../interfaces/CurrencyPairs';
import * as queryString from 'query-string';
import { Rates } from '../interfaces/Rates';

const fetchUrl = (ids: string[]) => {
	let query = queryString.stringify({
		currencyPairIds: ids
	});
	return `http://localhost:3000/rates?${query}`;
}

const fetchRates = (ids: string[]) => async (): Promise<Rates> => {
	try {
		let response = await fetch(fetchUrl(ids));;
		let json = await response.json();
		store.dispatch(updateRates(json.rates || {}, false));
		return json.rates || {} as Rates;
	} catch(err) {
		store.dispatch(updateRates({}, true));
	}
}

const interval = (pairs: CurrencyPairs) => {
	let ids = Object.keys(pairs);
	// Fetch imidiately, then every 15 secs
	fetchRates(ids)();
	setInterval(fetchRates(ids), 15 * 1000);
}

export const fetchValues = () => {

	const storeListener = () => {
		let state = store.getState() as State;
		if (Object.keys(state.pairs).length > 0) {
			// When pairs are loaded no need to listen anymore
			unsubscribe();
			interval(state.pairs);
		}
	}

	let unsubscribe = store.subscribe(storeListener);
}