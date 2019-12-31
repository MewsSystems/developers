import { SEARCH_CURRENCY } from './filter.constants'
import { ISearchCurrency } from './filter.models'

export type FilterState = {
  searchTerm: string
}

export type FilterAction = ISearchCurrency

const INITIAL_STATE: FilterState = {
  searchTerm: ''
}

export default (state = INITIAL_STATE, action: FilterAction) => {
  switch(action.type) {
    case SEARCH_CURRENCY:
      return {
        ...state,
        searchTerm: action.payload
      }
    default:
      return state
  }
}