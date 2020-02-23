import { AnimatedLine } from 'components/Loading/AnimatedLine'
import { render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test Loading AnimatedLine', () => {
  it('Tests snapshot', () => {
    expect(
      render(<AnimatedLine width="10rem" height="10rem" />)
    ).toMatchSnapshot()
  })
})
