import React from 'react'
import { shallow } from 'enzyme'
import { Loading } from '../Loading'

describe('Test Credits scene', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Loading />)).toMatchSnapshot()
  })
})
