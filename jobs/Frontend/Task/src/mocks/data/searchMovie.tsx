import { SearchMovieResponse } from "../../types";

export const searchMovieMockData: SearchMovieResponse = {
  page: 1,
  results: [
    {
      adult: false,
      backdrop_path: undefined,
      genre_ids: undefined,
      id: 1,
      original_language: "en",
      original_title: "The Lord of the Rings: The Two Towers",
      overview: "They are taking the Hobbits to Isengard",
      popularity: undefined,
      poster_path: undefined,
      release_date: "2002-12-18",
      title: "The Lord of the Rings: The Two Towers",
      video: false,
      vote_average: undefined,
      vote_count: undefined
    },
    {
      adult: false,
      backdrop_path: undefined,
      genre_ids: undefined,
      id: 2,
      original_language: "nl",
      original_title: "Back to the Future",
      overview: "Going back to the future in a Delorean",
      popularity: undefined,
      poster_path: undefined,
      release_date: "1985-12-12",
      title: "Back to the Future",
      video: false,
      vote_average: undefined,
      vote_count: undefined
    }
  ],
  total_pages: 1,
  total_results: 2
};
