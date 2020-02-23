import React from 'react'
import { shallow } from 'enzyme'
import { PageNotFound } from 'scenes/PageNotFound'

describe('Test PageNotFound scene', () => {
  it('Tests snapshot', () => {
    expect(shallow(<PageNotFound />)).toMatchSnapshot()
  })
})
