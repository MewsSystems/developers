import { addPair, setDisplay } from 'actions/currencyPairs'

test('should return addPair action object ', () => {
  const pair = 'AMD/GEL'
  const action = addPair(pair)
  expect(action).toEqual({
    type: 'ADD_PAIR',
    pair
  })
})

test('should return setDisplay action object ', () => {
  const pairs = ['AMD/GEL', 'PHP/DZD']
  const action = setDisplay(pairs)
  expect(action).toEqual({
    type: 'SET_DISPLAY',
    pairs
  })
})