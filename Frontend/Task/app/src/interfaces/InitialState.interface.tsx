import ICurrencyPairs from './CurrencyPairs.interface';
import ICurrencyPairsRates from './CurrencyPairsRates.interface'

export default interface IRootState {
  configuration: { 
    isLoading: boolean,
    currencyPairs : ICurrencyPairs
   },
  rates: {
    isLoading: boolean,
    currencyPairsRates: ICurrencyPairsRates
  }
  filter: Array<string>,
  error: boolean
}