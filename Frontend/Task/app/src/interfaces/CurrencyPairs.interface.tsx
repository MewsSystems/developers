import ICurrency from './Currency.interface'

export default interface ICurrencyPairs {
  [index: string]: Array<ICurrency>;
}