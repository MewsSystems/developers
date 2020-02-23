import { ErrorContent } from 'components/ErrorContent'
import { shallow } from 'enzyme'
import React from 'react'

describe('Test ErrorContent', () => {
  it('Tests snapshot', () => {
    expect(shallow(<ErrorContent title="test" text="test" />)).toMatchSnapshot()
  })
})
