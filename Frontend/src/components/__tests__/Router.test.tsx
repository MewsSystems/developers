import { Router } from 'components/Router'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test Router component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Router />)).toMatchSnapshot()
  })
})
