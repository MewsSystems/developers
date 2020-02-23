import { Loading } from '../Loading'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test Loading component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Loading />)).toMatchSnapshot()
  })
})
