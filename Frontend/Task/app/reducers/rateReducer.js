import { getRateTrend } from '../utils'
import { actionTypes, fetchState } from '../constants'

const initialState = {
  error: '',
  initialLoadSuccessful: false,
  rates: {},
  rateListState: null,
  trends: {},
}

export default (state = initialState, action) => {
  switch (action.type) {
    case actionTypes.RATE_LIST_DONE:
      const rates = action.rates
      const prevRates = state.rates

      return {
        ...state,
        error: '',
        initialLoadSuccessful: true,
        rates: rates,
        trends: getRateTrend(rates, prevRates),
        rateListState: fetchState.DONE,
      }

    case actionTypes.RATE_LIST_FETCHING:
      return {
        ...state,
        error: '',
        rateListState: fetchState.FETCHING,
      }

    case actionTypes.RATE_LIST_FAILED:
      return {
        ...state,
        error: action.message,
        rateListState: fetchState.FAILED,
      }

    default:
      return state
  }
}
