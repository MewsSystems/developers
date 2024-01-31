import type { Result } from "./interfaces/search-response"
import type { SimpleMovie } from "./interfaces/simple-movie"
import {
  IMAGE_BASE_URL,
  PLACEHOLDER_NO_IMG,
} from "../../../constants/constants"

export const transformToSimpleMovies = (
  resultsArray: Result[],
): SimpleMovie[] =>
  resultsArray.map(movie => ({
    id: movie.id,
    title: movie.title,
    image: movie.poster_path
      ? IMAGE_BASE_URL + movie.poster_path
      : PLACEHOLDER_NO_IMG,
  }))
