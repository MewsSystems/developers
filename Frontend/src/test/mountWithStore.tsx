import React from 'react'
import { Provider } from 'react-redux'
import { mockStore } from 'test/mockStore'
import { mount } from 'enzyme'
import { initialState, State } from 'state/rootReducer'

export const mountWithStore = (children: any, state: State = initialState) => {
  const store = mockStore(state)

  return mount(<Provider store={store}>{children}</Provider>)
}
