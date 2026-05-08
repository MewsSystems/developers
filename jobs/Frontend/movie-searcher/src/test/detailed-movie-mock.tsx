import { DetailedMovie } from "../models/tmdbModels";

export const detailedMovieMock: DetailedMovie = {
  adult: false,
  backdrop_path: "/path/to/backdrop.jpg",
  belongs_to_collection: null,
  budget: 200000000,
  genres: [
    { id: 28, name: "Action" },
    { id: 878, name: "Science Fiction" },
    { id: 53, name: "Thriller" },
  ],
  homepage: "https://www.example.com",
  id: 12345,
  imdb_id: "tt1375666",
  origin_country: ["US"],
  original_language: "en",
  original_title: "Inception",
  overview:
    "A thief who enters the dreams of others to steal secrets from their subconscious is given the inverse task of planting an idea into the mind of a CEO.",
  popularity: 87.4,
  poster_path: "/path/to/poster.jpg",
  production_companies: [
    { id: 1, name: "Warner Bros.", logo_path: "", origin_country: "" },
    {
      id: 2,
      name: "Legendary Entertainment",
      logo_path: "",
      origin_country: "",
    },
    { id: 3, name: "Syncopy", logo_path: "", origin_country: "" },
  ],
  production_countries: [
    { iso_3166_1: "US", name: "United States of America" },
  ],
  release_date: "2010-07-16",
  revenue: 829895144,
  runtime: 148,
  spoken_languages: [
    { iso_639_1: "en", name: "English", english_name: "" },
    { iso_639_1: "ja", name: "Japanese", english_name: "" },
  ],
  status: "Released",
  tagline: "Your mind is the scene of the crime.",
  title: "Inception",
  video: false,
  vote_average: 8.8,
  vote_count: 23524,
};
