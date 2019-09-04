export const filtersReducer = (state = [], action) => {
  switch (action.type) {
    case 'ADD_TO_FILTERS':
      return [...new Set([...state, action.pair])]

    case 'REMOVE_FROM_FILTERS':
      return state.filter((pair) => {
        return pair !== action.pair
      })


    default: return state
  }
}