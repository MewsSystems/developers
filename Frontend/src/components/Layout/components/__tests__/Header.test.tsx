import { Header } from '../Header'
import React from 'react'
import { shallow } from 'enzyme'

describe('Test Header', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Header />)).toMatchSnapshot()
  })
})
