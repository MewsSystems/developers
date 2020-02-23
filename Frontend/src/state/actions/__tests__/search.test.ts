import { mockStore } from 'test/mockStore'
import { mockState } from 'test/data/state'
import { setSearchValue } from '../search'

describe('Test Search thunk actions', () => {
  it('Test set search value', async () => {
    const store = mockStore(mockState)
    await store.dispatch(setSearchValue('test'))

    const actions = store.getActions()

    expect(actions[0]).toEqual(setSearchValue('test'))
  })
})
