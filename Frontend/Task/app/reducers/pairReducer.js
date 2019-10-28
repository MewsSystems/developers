import { actionTypes, fetchState } from '../constants'

const initialState = {
  error: '',
  currencyPairs: {},
  pairListState: null,
}

export default (state = initialState, action) => {
  switch (action.type) {
    case actionTypes.CURRENCY_LIST_DONE:
      return {
        ...state,
        error: '',
        currencyPairs: action.currencyPairs,
        pairListState: fetchState.DONE,
      }

    case actionTypes.CURRENCY_LIST_FETCHING:
      return {
        ...state,
        error: '',
        pairListState: fetchState.FETCHING,
      }

    case actionTypes.CURRENCY_LIST_FAILED:
      return {
        ...state,
        error: action.message,
        pairListState: fetchState.FAILED,
      }

    default:
      return state
  }
}
