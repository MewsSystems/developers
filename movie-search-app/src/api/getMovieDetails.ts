import CONSTANTS from "../constants/Config";
import type { Movie } from "../types/types";
import { options } from "./options";

export const getMovieDetails = (movie_id: number): Promise<Movie> => {
  return fetch(
    `${CONSTANTS.MOVIE_API}/movie/${movie_id}?language=en-US`,
    options
  )
    .then((response) => response.json())
    .catch((error) => console.error("Error fetching movie details:", error));
};
