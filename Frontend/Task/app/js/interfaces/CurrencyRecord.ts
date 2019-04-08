import { CurrencyTuple } from './CurrencyTuple';
import { Trend } from '../enums/Trend';

export interface CurrencyRecord {
	pair: CurrencyTuple,
	value?: number,
	trend?: Trend,
};