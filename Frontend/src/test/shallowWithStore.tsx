import React from 'react'
import * as ReactRedux from 'react-redux'
import { Provider } from 'react-redux'
import { mockStore } from 'test/mockStore'
import { shallow } from 'enzyme'
import { initialState, State } from 'state/rootReducer'
import { configurationMock } from './data/configuration'

export const shallowWithStore = (
  children: any,
  state: State = { ...initialState, configuration: configurationMock }
) => {
  const store = mockStore(state)

  jest.spyOn(ReactRedux, 'useDispatch').mockImplementation(() => store.dispatch)

  jest
    .spyOn(ReactRedux, 'useSelector')
    .mockImplementation((callback: any) => callback(state))

  return shallow(<Provider store={store}>{children}</Provider>)
    .dive()
    .dive()
}
