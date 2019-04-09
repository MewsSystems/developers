import { LOAD_RATES } from './actions'

const initialState = {
  rates: {},
  prevRates: {},
}

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case LOAD_RATES:
      const keys = Object.keys(action.payload)

      let prevRates = {}
      if (Object.values(state.rates).length === 0) {
        keys.forEach(item => {
          prevRates[item] = action.payload[item]
        })
      } else {
        keys.forEach(item => {
          prevRates[item] = state.rates[item]
        })
      }

      return Object.assign({}, state, { rates: action.payload, prevRates })
    default:
      return state
  }
}

export default reducer
