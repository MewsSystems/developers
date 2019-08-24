import ICurrencyPairRate from './CurrencyPairRate.interface';

export default interface ICurrencyPairsRates {
  [index: string]: ICurrencyPairRate,
}