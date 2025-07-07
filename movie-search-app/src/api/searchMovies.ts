import CONSTANTS from "../constants/Config";
import type { PaginatedMovieApiResponse } from "../types/types";
import { options } from "./options";

export const searchMovieList = (
  query: string,
  current_page: number
): Promise<PaginatedMovieApiResponse> => {
  return fetch(
    `${CONSTANTS.MOVIE_API}/search/movie?query=${query}&include_adult=false&language=en-US&page=${current_page}`,
    options
  )
    .then((response) => response.json())
    .catch((error) => console.error("Error fetching movie details:", error));
};
