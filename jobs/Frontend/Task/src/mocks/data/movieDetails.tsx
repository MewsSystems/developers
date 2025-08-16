import { MovieDetailsResponse } from "../../types";

export const movieDetailsMockData: MovieDetailsResponse = {
  adult: false,
  backdrop_path: undefined,
  belongs_to_collection: undefined,
  budget: undefined,
  genres: [
    {
      id: 12,
      name: "Adventure"
    },
    {
      id: 14,
      name: "Fantasy"
    },
    {
      id: 28,
      name: "Action"
    }
  ],
  homepage: undefined,
  id: 1,
  imdb_id: undefined,
  original_language: "en",
  original_title: "The Lord of the Rings: The Two Towers",
  overview: "They are taking the Hobbits to Isengard",
  popularity: undefined,
  poster_path: undefined,
  production_companies: undefined,
  production_countries: undefined,
  release_date: "2002-12-18",
  revenue: undefined,
  runtime: undefined,
  spoken_languages: undefined,
  status: "Released",
  tagline: "A new power is rising.",
  title: "The Lord of the Rings: The Two Towers",
  video: false,
  vote_average: undefined,
  vote_count: undefined
};
