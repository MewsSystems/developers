import { Search } from 'components/Search'
import { shallow, mount } from 'enzyme'
import React from 'react'

describe('Test Input component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Search />)).toMatchSnapshot()
  })

  it('Tests functionality', () => {
    const mockOnChange = jest.fn()
    const mockOnEnter = jest.fn()

    const wrapper = mount(
      <Search onChange={mockOnChange} onPressEnter={mockOnEnter} />
    )
    const input = wrapper.find('input')

    input.simulate('change', { event: { target: 'test' } })
    expect(mockOnChange).toBeCalled()

    input.simulate('keydown', { key: 'Enter' })
    expect(mockOnEnter).toBeCalled()
  })
})
