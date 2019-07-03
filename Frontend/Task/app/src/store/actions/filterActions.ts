import * as actionTypes from './actionTypes'

export const filterCurrencies = item => {
  return {
    type: actionTypes.FILTER,
    payload: item,
  }
}

export const resetFilter = () => {
  return {
    type: actionTypes.RESET_FILTER,
  }
}
