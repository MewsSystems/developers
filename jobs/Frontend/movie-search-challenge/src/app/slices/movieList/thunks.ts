import type { AppThunk } from "../../store"
import type { SearchResponse } from "./interfaces/search-response"
import { getDicoverMovies, getMovieSearch } from "../../../api/movies"
import {
  setMovies,
  startAddingResults,
  startLoadingMovies,
} from "./movieListSlice"
import { transformToSimpleMovies } from "./helpers"

export const getMovies = (): AppThunk => {
  return async (dispatch, getState) => {
    dispatch(startLoadingMovies())
    const { searchQuery: query } = getState().movieList

    const { data }: { data: SearchResponse } =
      query.length === 0
        ? await getDicoverMovies()
        : await getMovieSearch(query)

    dispatch(
      setMovies({
        movies: transformToSimpleMovies(data.results),
        page: data.page,
        totalPages: data.total_pages,
      }),
    )
  }
}

export const addResults = (): AppThunk => {
  return async (dispatch, getState) => {
    dispatch(startAddingResults())
    const { movies, page, searchQuery: query } = getState().movieList

    const { data }: { data: SearchResponse } = await getMovieSearch(
      query,
      page + 1,
    )

    const newMovies = transformToSimpleMovies(data.results)

    dispatch(
      setMovies({
        movies: movies.concat(newMovies),
        page: data.page,
        totalPages: data.total_pages,
      }),
    )
  }
}