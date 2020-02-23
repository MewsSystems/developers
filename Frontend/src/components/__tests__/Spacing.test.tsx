import { Spacing, StyledSpacing } from 'components/Spacing'
import { shallow, render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test Spacing component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Spacing />)).toMatchSnapshot()
  })
})

describe('Test StyledSpacing component', () => {
  it('Tests snapshot', () => {
    expect(render(<StyledSpacing />)).toMatchSnapshot()
  })
})
