import React from 'react'
import { shallowWithStore } from 'test/shallowWithStore'
import { Home } from '../Home'

describe('Test Home scene', () => {
  it('Tests snapshot', () => {
    expect(shallowWithStore(<Home />)).toMatchSnapshot()
  })
})
