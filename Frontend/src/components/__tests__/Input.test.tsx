import {
  Input,
  StyledInput,
  StyledCircleIcon,
  Container,
} from 'components/Input'
import { shallow, render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test Input component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Input />)).toMatchSnapshot()
    expect(shallow(<Input inputSize="large" />)).toMatchSnapshot()
  })

  it('Tests functionality', () => {
    const mockOnClear = jest.fn()
    const wrapper = shallow(<Input onClear={mockOnClear} allowClear />)
    const clearButton = wrapper.find(StyledCircleIcon)

    clearButton.simulate('click')
    expect(mockOnClear).toBeCalled()
  })
})

describe('Test StyledInput component', () => {
  it('Tests snapshot', () => {
    expect(render(<StyledInput inputSize="small" />)).toMatchSnapshot()
    expect(render(<StyledInput inputSize="large" />)).toMatchSnapshot()
  })
})

describe('Test Container component', () => {
  it('Tests snapshot', () => {
    expect(render(<Container />)).toMatchSnapshot()
    expect(render(<Container fullWidth />)).toMatchSnapshot()
  })
})

describe('Test StyledCircleIcon component', () => {
  it('Tests snapshot', () => {
    expect(render(<StyledCircleIcon icon="times" />)).toMatchSnapshot()
    expect(
      render(<StyledCircleIcon icon="times" inputSize="large" />)
    ).toMatchSnapshot()
  })
})
