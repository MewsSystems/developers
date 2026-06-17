import * as Factory from "factory.ts";
import { Movie, MovieResponse } from "./findMovieById";
import { MovieResult, SearchResponse } from "./searchMovies";

export const movieMock = Factory.Sync.makeFactory<Movie>({
  id: Factory.each((i) => i),
  title: "Harry Potter and the Chamber of Secrets",
  overview:
    "Cars fly, trees fight back, and a mysterious house-elf comes to warn Harry Potter at the start of his second year at Hogwarts. Adventure and danger await when bloody writing on a wall announces: The Chamber Of Secrets Has Been Opened. To save Hogwarts will require all of Harry, Ron and Hermione’s magical abilities and courage.",
  rating: 7.72,
  runtime: 161,
  productionCompanies: [
    "Warner Bros. Pictures",
    "Heyday Films",
    "1492 Pictures",
  ],
  languages: ["English"],
  imgSrc:
    "https://image.tmdb.org/t/p/original//1stUIsjawROZxjiCMtqqXqgfZWC.jpg",
  genres: ["Adventure", "Fantasy"],
  releaseDate: "2002-11-13",
});

export const movieResultMock = Factory.Sync.makeFactory<MovieResult>({
  id: Factory.each((i) => i),
  title: "Harry Potter and the Chamber of Secrets",
  overview:
    "Cars fly, trees fight back, and a mysterious house-elf comes to warn Harry Potter at the start of his second year at Hogwarts. Adventure and danger await when bloody writing on a wall announces: The Chamber Of Secrets Has Been Opened. To save Hogwarts will require all of Harry, Ron and Hermione’s magical abilities and courage.",
  rating: 7.72,
  imgSrc:
    "https://image.tmdb.org/t/p/original//1stUIsjawROZxjiCMtqqXqgfZWC.jpg",
});

export const searchResponseMock = Factory.Sync.makeFactory<SearchResponse>({
  page: 1,
  results: [
    {
      adult: false,
      backdrop_path: "/1stUIsjawROZxjiCMtqqXqgfZWC.jpg",
      genre_ids: [12, 14],
      id: 672,
      original_language: "en",
      original_title: "Harry Potter and the Chamber of Secrets",
      overview:
        "Cars fly, trees fight back, and a mysterious house-elf comes to warn Harry Potter at the start of his second year at Hogwarts. Adventure and danger await when bloody writing on a wall announces: The Chamber Of Secrets Has Been Opened. To save Hogwarts will require all of Harry, Ron and Hermione’s magical abilities and courage.",
      popularity: 128.912,
      poster_path: "/sdEOH0992YZ0QSxgXNIGLq1ToUi.jpg",
      release_date: "2002-11-13",
      title: "Harry Potter and the Chamber of Secrets",
      video: false,
      vote_average: 7.7,
      vote_count: 21218,
    },
  ],
  total_pages: 1,
  total_results: 1,
});

export const movieResponseMock = Factory.Sync.makeFactory<MovieResponse>({
  adult: false,
  backdrop_path: "/1stUIsjawROZxjiCMtqqXqgfZWC.jpg",
  belongs_to_collection: {
    id: 1241,
    name: "Harry Potter Collection",
    poster_path: "/eVPs2Y0LyvTLZn6AP5Z6O2rtiGB.jpg",
    backdrop_path: "/hWK8gTH2riuv65Ej43hPSeE16Mu.jpg",
  },
  budget: 100000000,
  genres: [
    {
      id: 12,
      name: "Adventure",
    },
    {
      id: 14,
      name: "Fantasy",
    },
  ],
  homepage:
    "https://www.warnerbros.com/movies/harry-potter-and-chamber-secrets/",
  id: 672,
  imdb_id: "tt0295297",
  origin_country: ["GB"],
  original_language: "en",
  original_title: "Harry Potter and the Chamber of Secrets",
  overview:
    "Cars fly, trees fight back, and a mysterious house-elf comes to warn Harry Potter at the start of his second year at Hogwarts. Adventure and danger await when bloody writing on a wall announces: The Chamber Of Secrets Has Been Opened. To save Hogwarts will require all of Harry, Ron and Hermione’s magical abilities and courage.",
  popularity: 128.912,
  poster_path: "/sdEOH0992YZ0QSxgXNIGLq1ToUi.jpg",
  production_companies: [
    {
      id: 174,
      logo_path: "/zhD3hhtKB5qyv7ZeL4uLpNxgMVU.png",
      name: "Warner Bros. Pictures",
      origin_country: "US",
    },
    {
      id: 437,
      logo_path: "/nu20mtwbEIhUNnQ5NXVhHsNknZj.png",
      name: "Heyday Films",
      origin_country: "GB",
    },
    {
      id: 436,
      logo_path: "/A7WCkG3F0NFvjGCwUnclpGdIu9E.png",
      name: "1492 Pictures",
      origin_country: "US",
    },
  ],
  production_countries: [
    {
      iso_3166_1: "GB",
      name: "United Kingdom",
    },
    {
      iso_3166_1: "US",
      name: "United States of America",
    },
  ],
  release_date: "2002-11-13",
  revenue: 876688482,
  runtime: 161,
  spoken_languages: [
    {
      english_name: "English",
      iso_639_1: "en",
      name: "English",
    },
  ],
  status: "Released",
  tagline: "Something evil has returned to Hogwarts!",
  title: "Harry Potter and the Chamber of Secrets",
  video: false,
  vote_average: 7.7,
  vote_count: 21218,
});
