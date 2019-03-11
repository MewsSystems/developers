import CurrencyPairInterface from './currencyPairInterface';

export default interface PairInterface {
  [pairId: string]: CurrencyPairInterface;
}
