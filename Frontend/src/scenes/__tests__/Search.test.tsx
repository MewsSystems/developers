import React from 'react'
import { Search } from 'scenes/Search'
import { shallowWithStore } from 'test/shallowWithStore'

describe('Test Search scene', () => {
  it('Tests snapshot', () => {
    expect(shallowWithStore(<Search />)).toMatchSnapshot()
  })
})
