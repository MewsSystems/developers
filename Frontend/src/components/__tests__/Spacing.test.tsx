import { Spacing } from 'components/Spacing'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test Spacing component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Spacing />)).toMatchSnapshot()
  })
})
