import { App } from 'components/Loading/App'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test Loading App', () => {
  it('Tests snapshot', () => {
    expect(shallow(<App />)).toMatchSnapshot()
  })
})
