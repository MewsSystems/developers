import { Card } from 'components/Card'
import { shallow } from 'enzyme'
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
