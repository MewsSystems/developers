import { MovieCard } from 'components/MovieCard'
import { shallow } from 'enzyme'
import 'jest-styled-components'
import React from 'react'

describe('Test Card component', () => {
  it('Tests snapshot', () => {
    expect(
      shallow(
        <MovieCard
          id={1}
          background="test.jpg"
          language="en"
          overview="test"
          title="test"
        />
      )
    ).toMatchSnapshot()
  })
})
