import { apiNock, ACCESS_CONTROL_HEADER } from 'test/apiNock'
import { configurationMock } from 'test/data/configuration'
import { mockStore } from 'test/mockStore'
import { mockState } from 'test/data/state'
import {
  hydrateConfiguration,
  hydrateConfigurationAction,
} from '../configuration'

describe('Test Configuration thunk actions', () => {
  it('Test hydrate configuration', async () => {
    apiNock()
      .get('/configuration')
      .reply(200, configurationMock, ACCESS_CONTROL_HEADER)

    const store = mockStore(mockState)
    await store.dispatch(hydrateConfiguration())

    const actions = store.getActions()

    expect(actions[0]).toEqual(hydrateConfigurationAction(configurationMock))
  })
})
