import {CONSTANTS} from 'client/actions/exchangeRatesActions'

const exchangeRateReducer = (state = {}, action) => {
  const {type, payload} = action

  switch (type) {
  case CONSTANTS.TOGGLE_FILTER:
    return {
      ...state,
      isFilterEnabled: !state.isFilterEnabled,
    }

  case CONSTANTS.UPDATE_FILTER_VALUE:
    return {
      ...state,
      filterValue: payload.value,
    }

  default:
    return state
  }
}

export default exchangeRateReducer
