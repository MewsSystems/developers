export const movie = {
  id: 1,
  title: "A",
  adult: false,
  backdrop_path: null,
  genre_ids: [],
  original_language: "en",
  original_title: "A",
  overview: "",
  poster_path: null,
  popularity: 0,
  release_date: "2023-01-01",
  video: false,
  vote_average: 0,
  vote_count: 0,
};

export const moviesPage1 = {
  page: 1,
  total_pages: 2,
  total_results: 2,
  results: [movie],
};

export const moviesPage2 = {
  page: 2,
  total_pages: 2,
  total_results: 2,
  results: [
    {
      ...movie,
      id: 2,
      title: "B",
      original_title: "B",
    },
  ],
};

export const movieDetails = {
  adult: false,
  backdrop_path: "/backdrop.jpg",
  belongs_to_collection: null,
  budget: 1000000,
  genres: [
    { id: 1, name: "Action" },
    { id: 2, name: "Adventure" },
  ],
  homepage: "https://example.com",
  id: 123,
  imdb_id: "tt1234567",
  original_language: "en",
  original_title: "Test Movie",
  overview: "This is a test movie overview with some interesting plot details.",
  popularity: 100,
  poster_path: "/poster.jpg",
  production_companies: [
    {
      id: 1,
      logo_path: null,
      name: "Test Studio",
      origin_country: "US",
    },
    {
      id: 2,
      logo_path: null,
      name: "Another Studio",
      origin_country: "UK",
    },
  ],
  production_countries: [{ iso_3166_1: "US", name: "United States" }],
  release_date: "2023-01-15",
  revenue: 5000000,
  runtime: 120,
  spoken_languages: [{ iso_639_1: "en", name: "English" }],
  status: "Released",
  tagline: "A test tagline for the movie",
  title: "Test Movie",
  video: false,
  vote_average: 7.5,
  vote_count: 1000,
};
