import { createStore, Action } from 'redux';
import { reducer } from './reducer';
import { Config } from '../interfaces/Config';
import { CurrencyPairs } from '../interfaces/CurrencyPairs';
import { Rates } from '../interfaces/Rates';

export interface State {
	pairs: CurrencyPairs,
	filteredIds?: string[],
	error?: boolean
};

export enum Actions {
	'CONFIGURATION',
	'RATES_UPDATE',
	'FILTER_UPDATE'
}

export interface ConfigAction {
	type: typeof Actions.CONFIGURATION;
	config: Config
}
export interface RateUpdateAction {
	type: typeof Actions.RATES_UPDATE;
	rates: Rates;
	error?: boolean
}
export interface FilterUpdate {
	type: typeof Actions.FILTER_UPDATE,
	filteredIds: string[]
}

export const addConfig = (config: Object) => ({
	type: Actions.CONFIGURATION,
	config
}) as ConfigAction;

export const updateRates = (rates: Rates, error: Boolean) => ({
	type: Actions.RATES_UPDATE,
	rates: rates,
	error: error
}) as RateUpdateAction;
export const updateFilter = (ids: string[]) => ({
	type: Actions.FILTER_UPDATE,
	filteredIds: ids
}) as FilterUpdate;


const store = createStore(reducer);
export default store;

