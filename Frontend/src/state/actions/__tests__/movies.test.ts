import { apiNock, ACCESS_CONTROL_HEADER } from 'test/apiNock'
import { mockStore } from 'test/mockStore'
import { mockState } from 'test/data/state'
import {
  receiveData,
  requestData,
  hydrateTrendingMovies,
  hydrateTrendingMoviesAction,
  setUpdatedAt,
} from '../movies'
import {
  trendingMoviesMock,
  moviesMockState,
  moviesMockStateRehydrated,
} from 'test/data/movies'

describe('Test trending movies thunk actions', () => {
  it('Tests hydrate trending movies', async () => {
    apiNock()
      .get('/trending/movie/week')
      .reply(200, trendingMoviesMock, ACCESS_CONTROL_HEADER)

    const store = mockStore(mockState)
    await store.dispatch(hydrateTrendingMovies())

    const actions = store.getActions()

    expect(actions[0]).toEqual(requestData())
    expect(actions[1]).toEqual(
      hydrateTrendingMoviesAction({
        rehydrate: false,
        data: trendingMoviesMock,
      })
    )
    expect(actions[2]).toEqual(receiveData())
  })

  it('Tests hydrating with already existing trending moveis in redux store', async () => {
    apiNock()
      .get('/trending/movie/week')
      .reply(200, trendingMoviesMock, ACCESS_CONTROL_HEADER)

    const store = mockStore({ ...mockState, movies: moviesMockState })
    await store.dispatch(hydrateTrendingMovies(false, 1))

    const actions = store.getActions()

    expect(actions.length).toBe(0)
  })

  it('Tests rehydrating with already existing trending moveis in redux store', async () => {
    apiNock()
      .get('/trending/movie/week?page=1')
      .reply(200, trendingMoviesMock, ACCESS_CONTROL_HEADER)

    const store = mockStore({ ...mockState, movies: moviesMockStateRehydrated })
    await store.dispatch(hydrateTrendingMovies(true, 1))

    const actions = store.getActions()

    expect(actions[0]).toEqual(requestData())
    expect(actions[1]).toEqual(
      hydrateTrendingMoviesAction({
        rehydrate: true,
        data: trendingMoviesMock,
      })
    )
    expect(actions[2]).toEqual(setUpdatedAt())
    expect(actions[3]).toEqual(receiveData())
  })
})
