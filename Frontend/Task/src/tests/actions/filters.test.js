import { addToFilters, removeFromFilters } from 'actions/filters'

test('should return addToFilters action object ', () => {
  const pair = 'AMD/GEL'
  const action = addToFilters(pair)
  expect(action).toEqual({
    type: 'ADD_TO_FILTERS',
    pair,
  })
})

test('should return removeFromFilters action object ', () => {
  const pair = 'AMD/GEL'
  const action = removeFromFilters(pair)
  expect(action).toEqual({
    type: 'REMOVE_FROM_FILTERS',
    pair,
  })
})