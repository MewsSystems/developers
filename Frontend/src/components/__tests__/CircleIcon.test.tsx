import { CircleIcon, Container } from 'components/CircleIcon'
import { shallow, render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test CircleIcon', () => {
  it('Tests snapshot', () => {
    expect(shallow(<CircleIcon icon="times" />)).toMatchSnapshot()
    expect(shallow(<CircleIcon icon="times" size="large" />)).toMatchSnapshot()
  })

  it('Tests functionality', () => {
    const mockOnClick = jest.fn()
    const wrapper = shallow(<CircleIcon icon="times" onClick={mockOnClick} />)

    wrapper.simulate('click')

    expect(mockOnClick).toBeCalled()
  })
})

describe('Test Container', () => {
  it('Tests snapshot', () => {
    expect(render(<Container />)).toMatchSnapshot()
    expect(
      render(<Container onClick={jest.fn()} size="large" />)
    ).toMatchSnapshot()
  })
})
