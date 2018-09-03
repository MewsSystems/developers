import type { CurrencyPair } from './reducer';

export type CurrencyPairRate = {
  id: string,
  rate: [number, number],
  currencyPair: CurrencyPair,
};
