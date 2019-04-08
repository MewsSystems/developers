import { CurrencyTuple } from './CurrencyTuple';

export interface Config {
	currencyPairs: {
		[key: string]: CurrencyTuple
	}
}