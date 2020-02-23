import { moviesReducer } from '../movies'
import {
  moviesInitialState,
  hydrateTrendingMoviesAction,
  receiveData,
  requestData,
  setUpdatedAt,
} from 'state/actions/movies'
import {
  trendingMoviesMock,
  moviesMockState,
  moviesMockStateRehydrated,
} from 'test/data/movies'

describe('Test movies reducer', () => {
  it('Tests initialization', () => {
    expect(moviesReducer).toBeDefined()
  })

  it('Tests initial state', () => {
    expect(moviesReducer(undefined, { type: '', payload: null })).toEqual(
      moviesInitialState
    )
  })

  it('Tests hydrate configuration', () => {
    const reduced = moviesReducer(
      moviesInitialState,
      hydrateTrendingMoviesAction({ rehydrate: true, data: trendingMoviesMock })
    )

    expect(reduced).toEqual(moviesMockStateRehydrated)
  })

  it('Tests hydrate configuration', () => {
    const reduced = moviesReducer(
      moviesInitialState,
      hydrateTrendingMoviesAction({
        rehydrate: false,
        data: trendingMoviesMock,
      })
    )

    expect(reduced).toEqual(moviesMockState)
  })

  it('Test request data', () => {
    const reduced = moviesReducer(moviesInitialState, requestData())

    expect(reduced.loading).toEqual(true)
  })

  it('Test receive data', () => {
    const reduced = moviesReducer(moviesInitialState, receiveData())

    expect(reduced.loading).toEqual(false)
  })

  it('Test set updated at', () => {
    jest
      .spyOn(global, 'Date')
      .mockImplementationOnce(() => new Date('2019-05-14T11:01:58.135Z') as any)

    const reduced = moviesReducer(moviesInitialState, setUpdatedAt())

    expect(reduced.updatedAt).toEqual(new Date('2019-05-14T11:01:58.135Z'))
  })
})
