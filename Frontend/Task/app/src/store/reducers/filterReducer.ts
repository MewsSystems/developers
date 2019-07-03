import * as actionTypes from '../actions/actionTypes'
import {
  filterCurrenciesInterface,
  resetFilterInterface,
} from '../actions/types'

const initialState = []

const addFilter = (state: Array<string>, action: filterCurrenciesInterface) => {
  if (!state.includes(action.payload)) {
    return [...state, action.payload]
  } else {
    return state.filter(i => i !== action.payload)
  }
}

const reducer = (
  state: Array<string> = initialState,
  action: filterCurrenciesInterface | resetFilterInterface
) => {
  switch (action.type) {
    case actionTypes.FILTER:
      return addFilter(state, action)
    case actionTypes.RESET_FILTER:
      return initialState
    default:
      return state
  }
}

export default reducer
