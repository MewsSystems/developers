import type { AppThunk } from "../../store"
import type { SearchResponse } from "./interfaces/search-response"
import type { SimpleMovie } from "./interfaces/simple-movie"
import { IMAGE_BASE_URL, getMovieSearch } from "../../../api/movies"
import { setMovies, startLoadingMovies } from "./movieListSlice"

export const getMovies = (page = 0, limit = 10): AppThunk => {
  return async dispatch => {
    dispatch(startLoadingMovies())

    const { data }: { data: SearchResponse } = await getMovieSearch("star wars")

    console.log(data)

    const simpleMovies: SimpleMovie[] = data.results.map(movie => ({
      id: movie.id,
      title: movie.original_title,
      image: IMAGE_BASE_URL + movie.poster_path,
    }))

    dispatch(setMovies({ movies: simpleMovies, page: page + 1 }))
  }
}
