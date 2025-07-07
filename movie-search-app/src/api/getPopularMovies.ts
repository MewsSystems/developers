import CONSTANTS from "../constants/Config";
import type { PaginatedMovieApiResponse } from "../types/types";
import { options } from "./options";

export const getPopularMovies = (
  currentPage: number
): Promise<PaginatedMovieApiResponse> => {
  return fetch(
    `${CONSTANTS.MOVIE_API}/movie/popular?language=en-US&page=${currentPage}`,
    options
  )
    .then((response) => response.json())
    .catch((error) => console.error("Error fetching popular movies:", error));
};
