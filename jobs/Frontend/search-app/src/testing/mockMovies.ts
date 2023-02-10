import {
  MovieDetailResult,
  MoviesSearchResult,
  MoviesSearchResponse,
} from "../app/types";

export const mockMovies: MoviesSearchResult[] = [
  {
    poster_path: "/cezWGskPY5x7GaglTTRN4Fugfb8.jpg",
    adult: false,
    overview:
      "When an unexpected enemy emerges and threatens global safety and security.",
    release_date: "2012-04-25",
    id: 24428,
    original_title: "The Avengers",
    original_language: "en",
    title: "The Avengers",
    backdrop_path: "/hbn46fQaRmlpBuUrEiFqv0GDL6Y.jpg",
    popularity: 7.353212,
    vote_count: 8503,
    video: false,
    vote_average: 7.33,
  },
  {
    poster_path: "/nTqwcAsxZyvp0ggSTWGcI3Qezrw.jpg",
    adult: false,
    overview:
      "When two acrobats are fired for fighting with punks in the audience.",
    release_date: "1979-03-15",
    id: 275663,
    original_title: "The Lama Avenger",
    original_language: "en",
    title: "The Lama Avenger",
    backdrop_path: null,
    popularity: 1.032625,
    vote_count: 0,
    video: false,
    vote_average: 0,
  },
];

export const mockMovieSearchResponse: MoviesSearchResponse = {
  page: 1,
  results: mockMovies,
  total_results: 2,
  total_pages: 1,
};

export const mockMovieDetail: MovieDetailResult = {
  adult: false,
  backdrop_path: "/fCayJrkfRaCRCTh8GqN30f8oyQF.jpg",
  belongs_to_collection: null,
  budget: 63000000,
  genres: [
    {
      id: 18,
      name: "Drama",
    },
  ],
  homepage: "",
  id: 550,
  original_language: "en",
  original_title: "Fight Club",
  overview:
    "A ticking-time-bomb insomniac and a slippery soap salesman channel primal male aggression into a shocking new form of therapy.",
  popularity: 0.5,
  poster_path: null,
  production_companies: [],
  production_countries: [],
  release_date: "1999-10-12",
  revenue: 100853753,
  runtime: 139,
  spoken_languages: [],
  status: "Released",
  tagline:
    "How much can you know about yourself if you've never been in a fight?",
  title: "Fight Club",
  video: false,
  vote_average: 7.8,
  vote_count: 3439,
};
