import { Card } from 'components/Loading/Card'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test Loading Card', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Card />)).toMatchSnapshot()
  })
})
