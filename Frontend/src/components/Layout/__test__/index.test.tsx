import { Layout } from '../'
import React from 'react'
import { shallow } from 'enzyme'

describe('Test Layout', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Layout>test</Layout>)).toMatchSnapshot()
  })
})
