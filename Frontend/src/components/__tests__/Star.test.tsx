import React from 'react'
import { shallow } from 'enzyme'
import { Star } from 'components/Star'

describe('Test Star component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Star type="empty" />)).toMatchSnapshot()
    expect(shallow(<Star type="half" />)).toMatchSnapshot()
    expect(shallow(<Star type="full" />)).toMatchSnapshot()
  })
})
