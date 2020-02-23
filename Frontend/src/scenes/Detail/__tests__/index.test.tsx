import React from 'react'
import { shallowWithStore } from 'test/shallowWithStore'
import { Detail } from '../index'

describe('Test Detail scene', () => {
  it('Tests snapshot', () => {
    expect(shallowWithStore(<Detail />)).toMatchSnapshot()
  })
})
