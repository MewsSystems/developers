export const currencyPairsReducer = (state = [], action) => {
  switch (action.type) {
    case 'ADD_PAIR':
      return [
        ...state,
        action.pair
      ]
    case 'FILTER_PAIR':
      return state.map((pair) => {
        if (pair.name === action.pairName) {
          return {
            ...pair,
            ...pair.display = !pair.display
          }
        }
        return pair
      })

    default: return state
  }
}