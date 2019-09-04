export const addToFilters = (pair) => ({
  type: 'ADD_TO_FILTERS',
  pair,
})

export const removeFromFilters = (pair) => ({
  type: 'REMOVE_FROM_FILTERS',
  pair,
})
