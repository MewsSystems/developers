import { searchReducer } from '../search'
import { searchInitialState, setSearchValue } from 'state/actions/search'

describe('Test configuration reducer', () => {
  it('Tests initialization', () => {
    expect(searchReducer).toBeDefined()
  })

  it('Tests initial state', () => {
    expect(searchReducer(undefined, { type: '', payload: null })).toEqual(
      searchInitialState
    )
  })

  it('Tests set search value', () => {
    const reduced = searchReducer(searchInitialState, setSearchValue('test'))

    expect(reduced.value).toEqual('test')
  })
})
