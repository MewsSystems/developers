export const currencyPairsReducer = (state = [], action) => {
  switch (action.type) {
    case 'ADD_PAIR':
      return [
        ...state,
        action.pair
      ]
    case 'SET_DISPLAY':
      return state.map((pair) => {
        if (action.pairs.includes(pair.name)) {
          return {
            ...pair,
            ...pair.display = true
          }
        } else {
          return {
            ...pair,
            ...pair.display = false
          }
        }
      })
    default: return state
  }
}