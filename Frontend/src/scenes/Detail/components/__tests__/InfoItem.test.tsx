import React from 'react'
import { shallow } from 'enzyme'
import { InfoItem } from '../InfoItem'

describe('Test InfoItem component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<InfoItem label="test" />)).toMatchSnapshot()
  })
})
