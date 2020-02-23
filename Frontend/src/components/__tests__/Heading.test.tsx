import { Heading, StyledHeading } from 'components/Heading'
import { shallow, render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test Heading component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Heading level={1} />)).toMatchSnapshot()
  })
})

describe('Test StyledHeading component', () => {
  it('Tests snapshot', () => {
    expect(
      render(<StyledHeading fontSize="2rem" margin="1rem" color="black" />)
    ).toMatchSnapshot()
  })
})
