import { Navigation } from 'components/Navigation'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test Navigation component', () => {
  it('Tests snapshot', () => {
    expect(
      shallow(
        <Navigation items={[{ key: 'test', title: 'Test', to: '/test' }]} />
      )
    ).toMatchSnapshot()
  })
})
