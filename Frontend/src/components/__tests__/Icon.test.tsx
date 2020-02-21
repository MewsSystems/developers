import { Icon, Container } from 'components/Icon'
import { shallow, render } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test Icon component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Icon icon="times" />)).toMatchSnapshot()
  })

  it('Tests functionality', () => {
    const mockOnClear = jest.fn()
    const wrapper = shallow(<Icon icon="times" onClick={mockOnClear} />)

    wrapper.simulate('click')
    expect(mockOnClear).toBeCalled()
  })
})

describe('Test Container component', () => {
  it('Tests snapshot', () => {
    expect(render(<Container />)).toMatchSnapshot()
    expect(render(<Container hoverColor="#FFF" />)).toMatchSnapshot()
  })
})
