import React from 'react'
import { Rating } from 'components/Rating'
import { shallow } from 'enzyme'

describe('Test rating component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Rating rating={3.5} total={100} />)).toMatchSnapshot()
  })

  it('Test rating functionality', () => {
    const wrapper1 = shallow(<Rating rating={4} total={100} />)
    const wrapper2 = shallow(<Rating rating={4.5} total={100} />)
    const wrapper3 = shallow(<Rating rating={5} total={100} />)

    expect(wrapper1.find('[type="empty"]').length).toBe(1)
    expect(wrapper2.find('[type="empty"]').length).toBe(0)
    expect(wrapper2.find('[type="half"]').length).toBe(1)
    expect(wrapper3.find('[type="empty"]').length).toBe(0)
  })
})
