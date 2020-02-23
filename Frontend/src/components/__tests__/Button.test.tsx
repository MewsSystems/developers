import { Button, StyledButton } from 'components/Button'
import { shallow, render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test Button', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Button />)).toMatchSnapshot()
  })

  it('Tests functionality', () => {
    const mockOnClick = jest.fn()
    const wrapper = shallow(<Button onClick={mockOnClick} />)

    wrapper.simulate('click')

    expect(mockOnClick).toBeCalled()
  })
})

describe('Test StyledButton', () => {
  it('Tests snapshot', () => {
    expect(render(<StyledButton />)).toMatchSnapshot()
    expect(render(<StyledButton disabled />)).toMatchSnapshot()
    expect(render(<StyledButton variant="secondary" />)).toMatchSnapshot()
  })
})
