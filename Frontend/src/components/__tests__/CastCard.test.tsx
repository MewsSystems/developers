import { CastCard } from 'components/CastCard'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test Card component', () => {
  it('Tests snapshot', () => {
    expect(
      shallow(<CastCard background="test.jpg" name="test" role="test" />)
    ).toMatchSnapshot()
  })
})
