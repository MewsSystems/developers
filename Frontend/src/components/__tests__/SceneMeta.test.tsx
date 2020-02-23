import { SceneMeta } from 'components/SceneMeta'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test SceneMeta component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<SceneMeta title="test" />)).toMatchSnapshot()
  })
})
