import { makeStore, type AppStore } from "../../store"
import {
  movieListSlice,
  startLoadingMovies,
  type MovieListState,
  startAddingResults,
  setSearchQuery,
  setMovies,
  initialState,
} from "./movieListSlice"

interface LocalTestContext {
  store: AppStore
}

describe<LocalTestContext>("movie slice", it => {
  const initialTestState: MovieListState = {
    page: 1,
    movies: [
      {
        id: 609681,
        title: "The Marvels",
        image:
          "https://image.tmdb.org/t/p/w1280/9GBhzXMFjgcZ3FdR9w3bUMMTps5.jpg",
      },
      {
        id: 955916,
        title: "Lift",
        image:
          "https://image.tmdb.org/t/p/w1280/gma8o1jWa6m0K1iJ9TzHIiFyTtI.jpg",
      },
      {
        id: 753342,
        title: "Napoleon",
        image:
          "https://image.tmdb.org/t/p/w1280/jE5o7y9K6pZtWNNMEw3IdpHuncR.jpg",
      },
    ],
    isLoading: false,
    isLoadingMoreResults: false,
    morePages: true,
    searchQuery: "",
  }

  beforeEach<LocalTestContext>(context => {
    const store = makeStore({ movieList: initialTestState })

    context.store = store
  })

  it("should handle initial state", () => {
    expect(
      movieListSlice.reducer(undefined, { type: "unknown" }),
    ).toStrictEqual(initialState)
  })

  it("should handle start loading movies", ({ store }) => {
    expect(store.getState().movieList.isLoading).toBe(false)

    store.dispatch(startLoadingMovies())

    expect(store.getState().movieList.isLoading).toBe(true)
  })

  it("should handle start adding results", ({ store }) => {
    expect(store.getState().movieList.isLoadingMoreResults).toBe(false)

    store.dispatch(startAddingResults())

    expect(store.getState().movieList.isLoadingMoreResults).toBe(true)
  })

  it("should handle set Movies", ({ store }) => {
    const newMoviesPayload = {
      page: 2,
      movies: [
        {
          id: 753342,
          title: "Napoleon",
          image:
            "https://image.tmdb.org/t/p/w1280/jE5o7y9K6pZtWNNMEw3IdpHuncR.jpg",
        },
      ],
      totalPages: 10,
    }

    const newState = {
      page: newMoviesPayload.page,
      movies: newMoviesPayload.movies,
      isLoading: false,
      isLoadingMoreResults: false,
      morePages: true,
      searchQuery: "",
    }

    expect(store.getState().movieList).toBe(initialTestState)

    store.dispatch(setMovies(newMoviesPayload))

    expect(store.getState().movieList).toStrictEqual(newState)
  })

  it("should handle set search query", ({ store }) => {
    const newQuery = "test"

    expect(store.getState().movieList.searchQuery).toBe(
      initialTestState.searchQuery,
    )

    store.dispatch(setSearchQuery({ searchQuery: newQuery }))

    expect(store.getState().movieList.searchQuery).toBe(newQuery)
  })
})
