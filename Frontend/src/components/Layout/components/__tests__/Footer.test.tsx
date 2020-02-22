import { Footer } from '../Footer'
import React from 'react'
import { shallow } from 'enzyme'

describe('Test Footer', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Footer />)).toMatchSnapshot()
  })
})
