import { Header } from '../Header'
import React from 'react'
import { shallowWithStore } from 'test/shallowWithStore'

describe('Test Header', () => {
  it('Tests snapshot', () => {
    expect(shallowWithStore(<Header />)).toMatchSnapshot()
  })
})
