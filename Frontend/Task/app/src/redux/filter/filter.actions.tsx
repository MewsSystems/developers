import {SEARCH_CURRENCY} from './filter.constants'

export const searchCurrency = (value = '') => ({
  type: SEARCH_CURRENCY,
  payload: value
})