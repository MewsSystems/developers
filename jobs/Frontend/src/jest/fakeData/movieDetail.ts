import { MovieDetail } from "@/scenes/MovieDetail/services/types";

export const testMovieDetail: MovieDetail = {
  adult: false,
  backdrop_path: "test.jpg",
  belongs_to_collection: null,
  budget: 100,
  genres: [
    {
      id: 1,
      name: "Test Genre",
    },
    {
      id: 2,
      name: "Test Genre 2",
    },
  ],
  homepage: "https://www.test.com",
  id: 1,
  imdb_id: "tt123456",
  original_language: "en",
  original_title: "Test Movie",
  overview: "Test overview",
  popularity: 100,
  poster_path: "test.jpg",
  production_companies: [
    {
      id: 1,
      logo_path: "test.jpg",
      name: "Test Company",
      origin_country: "US",
    },
  ],
  production_countries: [
    {
      iso_3166_1: "US",
      name: "United States of America",
    },
  ],
  release_date: "2021-01-01",
  revenue: 100,
  runtime: 100,
  spoken_languages: [
    {
      iso_639_1: "en",
      name: "English",
    },
  ],
  status: "Released",
  tagline: "Test tagline",
  title: "Test Movie",
  video: false,
  vote_average: 5,
  vote_count: 100,
};
