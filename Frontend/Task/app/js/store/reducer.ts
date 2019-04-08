import { Actions, ConfigAction, State, RateUpdateAction, FilterUpdate } from './store';
import { CurrencyPairs } from '../interfaces/CurrencyPairs';
import { Config } from '../interfaces/Config';
import { Rates } from '../interfaces/Rates';
import { Trend } from '../enums/Trend';
import { storeRestore } from '../helpers/storeBakcup';
import { CurrencyRecord } from '../interfaces/CurrencyRecord';

// Try to load store from localStorage
const initialState = storeRestore() || {
	pairs: {}
} as State;

const parsePairs = (rawConfig: Config, oldPairs: CurrencyPairs) => {
	let pairs = {} as CurrencyPairs;
	Object.keys(rawConfig.currencyPairs).forEach(key => {
		let oldPair = oldPairs[key] || {} as CurrencyRecord;
		pairs[key] = {
			pair: rawConfig.currencyPairs[key],
			value: oldPair.value,
			trend: oldPair.trend
		};
	});
	return pairs;
}

const determineTrend = (oldVal: number, newVal: number): Trend => {
	if (oldVal < newVal) return Trend.GROWING;
	else if (oldVal > newVal) return Trend.DECLINING;
	else if (oldVal === newVal) return Trend.STAGNATING
}

const infusePairs = (pairs: CurrencyPairs, rates: Rates) => {
	Object.keys(rates).forEach(key => {
		let pair = pairs[key];
		if (!pair) return;
		let oldValue = pair.value;
		pair.value = rates[key];
		pair.trend = determineTrend(oldValue, pair.value) || pair.trend;
		console.log(`Currency ${pair.pair[0].code}/${pair.pair[1].code} changed from ${oldValue} to ${pair.value} with trend ${pair.trend}`)
	});
	console.log('_______________________________')
	return pairs;
}

export const reducer = (state = initialState, action: ConfigAction|RateUpdateAction|FilterUpdate): State => {
	if (action.type === Actions.CONFIGURATION) {
		return {
			...state,
			pairs: parsePairs(action.config, state.pairs)
		}
	} else if (action.type === Actions.RATES_UPDATE) {
		return {
			...state,
			pairs: {
				...infusePairs(state.pairs, action.rates)
			},
			error: action.error
		}
	} else if (action.type === Actions.FILTER_UPDATE) {
		return {
			...state,
			filteredIds: action.filteredIds || []
		}
	}
	return state;
};