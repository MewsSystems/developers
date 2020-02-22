import { Card, StyledCard } from 'components/Card'
import { shallow, render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test Card component', () => {
  it('Tests snapshot', () => {
    expect(
      shallow(
        <Card
          id={1}
          background="test.jpg"
          language="en"
          overview="test"
          title="test"
        />
      )
    ).toMatchSnapshot()
  })
})

describe('Test StyledCard component', () => {
  it('Tests snapshot', () => {
    expect(render(<StyledCard background="test.jpg" />)).toMatchSnapshot()
  })
})
