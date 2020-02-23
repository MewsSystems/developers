import React from 'react'
import { shallow } from 'enzyme'
import { movieDetailMock } from 'test/data/movies'
import { Info } from '../Info'

describe('Test Info component', () => {
  it('Tests snapshot', () => {
    expect(shallow(<Info data={movieDetailMock} />)).toMatchSnapshot()
  })
})
