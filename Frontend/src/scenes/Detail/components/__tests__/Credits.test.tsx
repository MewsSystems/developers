import React from 'react'
import { shallowWithStore } from 'test/shallowWithStore'
import { Credits } from '../Credits'

describe('Test Credits scene', () => {
  it('Tests snapshot', () => {
    expect(shallowWithStore(<Credits id={1} />)).toMatchSnapshot()
  })
})
