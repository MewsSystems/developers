import ICurrencyPairs from './CurrencyPairs.interface'
import ICurrencyPairsRates from './CurrencyPairsRates.interface'

export default interface ICurrencyPairsRateList {
  configuration : { 
    isLoading: boolean,
    currencyPairs : ICurrencyPairs
   },
  filter: Array<string>,
  rates: {
    isLoading: boolean,
    currencyPairsRates: ICurrencyPairsRates
  }
}
