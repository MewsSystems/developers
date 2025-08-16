import { fireEvent, screen } from "@testing-library/react"
import { renderWithProviders } from "./utils/test-utils"
import * as api from "./api/movies"
import App from "./App"
import type { OriginalLanguage } from "./app/slices/movieList/interfaces/search-response"

describe("App component test", () => {
  afterEach(() => {
    vi.restoreAllMocks()
  })

  const resolvedValue = {
    data: {
      page: 1,
      results: [
        {
          adult: false,
          backdrop_path: "/criPrxkTggCra1jch49jsiSeXo1.jpg",
          genre_ids: [878, 12, 28],
          id: 609681,
          original_language: "en" as OriginalLanguage,
          original_title: "The Marvels",
          overview:
            "Carol Danvers, aka Captain Marvel, has reclaimed her identity from the tyrannical Kree and taken revenge on the Supreme Intelligence. But unintended consequences see Carol shouldering the burden of a destabilized universe. When her duties send her to an anomalous wormhole linked to a Kree revolutionary, her powers become entangled with that of Jersey City super-fan Kamala Khan, aka Ms. Marvel, and Carol’s estranged niece, now S.A.B.E.R. astronaut Captain Monica Rambeau. Together, this unlikely trio must team up and learn to work in concert to save the universe.",
          popularity: 3327.208,
          poster_path: "/9GBhzXMFjgcZ3FdR9w3bUMMTps5.jpg",
          release_date: "2023-11-08",
          title: "The Marvels",
          video: false,
          vote_average: 6.4,
          vote_count: 1138,
        },
        {
          adult: false,
          backdrop_path: "/rz8GGX5Id2hCW1KzAIY4xwbQw1w.jpg",
          genre_ids: [35, 80],
          id: 955916,
          original_language: "en" as OriginalLanguage,
          original_title: "Lift",
          overview:
            "An international heist crew, led by Cyrus Whitaker, race to lift $500 million in gold from a passenger plane at 40,000 feet.",
          popularity: 1745.578,
          poster_path: "/gma8o1jWa6m0K1iJ9TzHIiFyTtI.jpg",
          release_date: "2024-01-10",
          title: "Lift",
          video: false,
          vote_average: 6.4,
          vote_count: 433,
        },
      ],
      total_pages: 42100,
      total_results: 841999,
    },
  }

  const resolvedDetails = {
    data: {
      adult: false,
      backdrop_path: "/criPrxkTggCra1jch49jsiSeXo1.jpg",
      belongs_to_collection: {
        id: 623911,
        name: "Captain Marvel Collection",
        poster_path: "/mHiMmryCureDvoAOlGP6o3oXT8Y.jpg",
        backdrop_path: "/zJrQR9g3hnpC8FY4xCXYVg3ztsA.jpg",
      },
      budget: 274800000,
      genres: [
        {
          id: 878,
          name: "Science Fiction",
        },
        {
          id: 12,
          name: "Adventure",
        },
        {
          id: 28,
          name: "Action",
        },
      ],
      homepage: "https://www.marvel.com/movies/the-marvels",
      id: 609681,
      imdb_id: "tt10676048",
      original_language: "en",
      original_title: "The Marvels",
      overview:
        "Carol Danvers, aka Captain Marvel, has reclaimed her identity from the tyrannical Kree and taken revenge on the Supreme Intelligence. But unintended consequences see Carol shouldering the burden of a destabilized universe. When her duties send her to an anomalous wormhole linked to a Kree revolutionary, her powers become entangled with that of Jersey City super-fan Kamala Khan, aka Ms. Marvel, and Carol’s estranged niece, now S.A.B.E.R. astronaut Captain Monica Rambeau. Together, this unlikely trio must team up and learn to work in concert to save the universe.",
      popularity: 3327.208,
      poster_path: "/9GBhzXMFjgcZ3FdR9w3bUMMTps5.jpg",
      production_companies: [
        {
          id: 420,
          logo_path: "/hUzeosd33nzE5MCNsZxCGEKTXaQ.png",
          name: "Marvel Studios",
          origin_country: "US",
        },
        {
          id: 176762,
          logo_path: null,
          name: "Kevin Feige Productions",
          origin_country: "US",
        },
      ],
      production_countries: [
        {
          iso_3166_1: "US",
          name: "United States of America",
        },
      ],
      release_date: "2023-11-08",
      revenue: 205600000,
      runtime: 105,
      spoken_languages: [
        {
          english_name: "English",
          iso_639_1: "en",
          name: "English",
        },
        {
          english_name: "Urdu",
          iso_639_1: "ur",
          name: "اردو",
        },
      ],
      status: "Released",
      tagline: "Higher. Further. Faster. Together.",
      title: "The Marvels",
      video: false,
      vote_average: 6.368,
      vote_count: 1151,
    },
  }

  it("should have correct initial render", async () => {
    const getSpy = vi.spyOn(api, "getDicoverMovies")
    const mock = vi.fn().mockReturnValue(resolvedValue)
    getSpy.mockImplementation(mock)

    renderWithProviders(<App />)

    expect(screen.getByPlaceholderText("Search")).toBeInTheDocument()
    expect(
      await screen.findByText(resolvedValue.data.results[0].title),
    ).toBeInTheDocument()

    expect(getSpy).toHaveBeenCalledWith()
  })

  it("should redirect to movie details when a card is clicked", async () => {
    const getDicoverMoviesSpy = vi.spyOn(api, "getDicoverMovies")
    const mock = vi.fn().mockReturnValue(resolvedValue)
    getDicoverMoviesSpy.mockImplementation(mock)

    const getMovieDetailsSpy = vi.spyOn(api, "getMovieDetails")
    const mock2 = vi.fn().mockReturnValue(resolvedDetails)
    getMovieDetailsSpy.mockImplementation(mock2)

    renderWithProviders(<App />)

    const movieCard = await screen.findByText(
      resolvedValue.data.results[0].title,
    )

    fireEvent.click(movieCard)

    const overViewSection = await screen.findByText(
      resolvedValue.data.results[0].overview,
    )

    expect(overViewSection).toBeInTheDocument()
  })
})
