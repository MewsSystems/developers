import ICurrency from './Currency.interface'

export default interface IFilterCheckbox{
  id: string,
  filter: Array<string>,
  currencyPair: Array<ICurrency>,
  toggleFilter: (pairsID: string, isChecked: boolean) => void 
} 