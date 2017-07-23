const exchangeRateReducer = (state = {}, action) => {
  const {type, payload} = action

  switch (type) {
  case 'TOGGLE_FILTER':
    return {
      ...state,
      isFilterEnabled: payload.value,
    }

  case 'UPDATE_FILTER_VALUE':
    return {
      ...state,
      filterValue: payload.value,
    }

  default:
    return state
  }
}

export default exchangeRateReducer
