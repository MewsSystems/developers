import ICurrencyPairs from './CurrencyPairs.interface'

export default interface ICurrencyPairsFilter {
  configuration : { 
    isLoading: boolean,
    currencyPairs : ICurrencyPairs
   },
  filter: Array<string>,
  toggleFilter: (pairsID: string, isChecked: boolean) => void 
}

