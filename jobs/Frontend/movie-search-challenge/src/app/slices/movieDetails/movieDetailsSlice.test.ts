import { makeStore, type AppStore } from "../../store"
import {
  initialState,
  movieDetailsSlice,
  startLoadingDetails,
  type MovieDetailsState,
  setDetails,
} from "./movieDetailsSlice"

interface LocalTestContext {
  store: AppStore
}

describe<LocalTestContext>("movie details slice", it => {
  const initialTestState: MovieDetailsState = {
    details: {
      image: "https://image.tmdb.org/t/p/w1280/fNOH9f1aA7XRTzl1sAOx9iF553Q.jpg",
      title: "Back to the Future",
      tagline:
        "He was never in time for his classes. He wasn't in time for his dinner. Then one day...he wasn't in his time at all.",
      language: "en",
      length: 116,
      rate: 8.314,
      budget: 19000000,
      release_date: "1985-07-03",
      genres: [
        {
          id: 12,
          name: "Adventure",
        },
        {
          id: 35,
          name: "Comedy",
        },
        {
          id: 878,
          name: "Science Fiction",
        },
      ],
      overview:
        "Eighties teenager Marty McFly is accidentally sent back in time to 1955, inadvertently disrupting his parents' first meeting and attracting his mother's romantic interest. Marty must repair the damage to history by rekindling his parents' romance and - with the help of his eccentric inventor friend Doc Brown - return to 1985.",
    },
    isLoading: false,
  }

  beforeEach<LocalTestContext>(context => {
    const store = makeStore({ movieDetails: initialTestState })

    context.store = store
  })

  it("should handle initial state", () => {
    expect(
      movieDetailsSlice.reducer(undefined, { type: "unknown" }),
    ).toStrictEqual(initialState)
  })

  it("should handle start loading details", ({ store }) => {
    expect(store.getState().movieDetails.isLoading).toBe(false)

    store.dispatch(startLoadingDetails())

    expect(store.getState().movieDetails.isLoading).toBe(true)
  })

  it("should handle set details", ({ store }) => {
    const newDetailsPayload = {
      movieDetails: {
        image:
          "https://image.tmdb.org/t/p/w1280/or06FN3Dka5tukK1e9sl16pB3iy.jpg",
        title: "Avengers: Endgame",
        tagline: "Avenge the fallen.",
        language: "en",
        length: 181,
        rate: 8.258,
        budget: 356000000,
        release_date: "2019-04-24",
        genres: [
          {
            id: 12,
            name: "Adventure",
          },
          {
            id: 878,
            name: "Science Fiction",
          },
          {
            id: 28,
            name: "Action",
          },
        ],
        overview:
          "After the devastating events of Avengers: Infinity War, the universe is in ruins due to the efforts of the Mad Titan, Thanos. With the help of remaining allies, the Avengers must assemble once more in order to undo Thanos' actions and restore order to the universe once and for all, no matter what consequences may be in store.",
      },
    }

    const newState: MovieDetailsState = {
      details: newDetailsPayload.movieDetails,
      isLoading: false,
    }

    expect(store.getState().movieDetails).toBe(initialTestState)

    store.dispatch(setDetails(newDetailsPayload))

    expect(store.getState().movieDetails).toStrictEqual(newState)
  })
})
