import type { AppThunk } from "../../store"
import type { SearchResponse } from "./interfaces/search-response"
import type { SimpleMovie } from "./interfaces/simple-movie"
import {
  IMAGE_BASE_URL,
  getDicoverMovies,
  getMovieSearch,
} from "../../../api/movies"
import { setMovies, startLoadingMovies } from "./movieListSlice"

export const getMovies = (): AppThunk => {
  return async (dispatch, getState) => {
    dispatch(startLoadingMovies())
    const { searchQuery: query, page } = getState().movieList

    const { data }: { data: SearchResponse } =
      query.length === 0
        ? await getDicoverMovies()
        : await getMovieSearch(query)

    const simpleMovies: SimpleMovie[] = data.results.map(movie => ({
      id: movie.id,
      title: movie.original_title,
      image: IMAGE_BASE_URL + movie.poster_path,
    }))

    dispatch(setMovies({ movies: simpleMovies, page }))
  }
}
