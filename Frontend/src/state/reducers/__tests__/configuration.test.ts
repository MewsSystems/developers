import { configurationReducer } from '../configuration'
import {
  configurationInitialState,
  hydrateConfigurationAction,
} from 'state/actions/configuration'
import { configurationMock } from 'test/data/configuration'

describe('Test configuration reducer', () => {
  it('Tests initialization', () => {
    expect(configurationReducer).toBeDefined()
  })

  it('Tests initial state', () => {
    expect(
      configurationReducer(undefined, { type: '', payload: null })
    ).toEqual(configurationInitialState)
  })

  it('Tests hydrate configuration', () => {
    const reduced = configurationReducer(
      configurationInitialState,
      hydrateConfigurationAction(configurationMock)
    )

    expect(reduced).toEqual(configurationMock)
  })
})
