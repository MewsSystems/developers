import { MovieList } from 'components/MovieList'
import 'jest-styled-components'
import React from 'react'
import { movieMock } from 'test/data/movies'
import { shallowWithStore } from 'test/shallowWithStore'

describe('Test MovieList component', () => {
  it('Tests snapshot with no items', () => {
    expect(
      shallowWithStore(
        <MovieList
          items={[]}
          loading={false}
          onLoadMore={jest.fn()}
          onMovieClick={jest.fn()}
          page={0}
          totalPages={0}
        />
      )
    ).toMatchSnapshot()
  })

  it('Tests snapshot with items', () => {
    expect(
      shallowWithStore(
        <MovieList
          items={[movieMock]}
          loading={false}
          onLoadMore={jest.fn()}
          onMovieClick={jest.fn()}
          page={1}
          totalPages={1}
        />
      )
    ).toMatchSnapshot()
  })
})
