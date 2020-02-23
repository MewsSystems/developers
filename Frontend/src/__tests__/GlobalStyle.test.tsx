import GlobalStyle from 'GlobalStyle'
import { render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test GlobalStyle', () => {
  it('Tests snapshot', () => {
    expect(render(<GlobalStyle />)).toMatchSnapshot()
  })
})
