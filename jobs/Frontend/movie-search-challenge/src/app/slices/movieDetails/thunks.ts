import type { AppThunk } from "../../store"
import type { SimpleMovieDetails } from "./interfaces/simple-movie-details"
import type { MovieDetailsResponse } from "./interfaces/movie-details-response"
import { setDetails, startLoadingDetails } from "./movieDetailsSlice"
import { IMAGE_BASE_URL, getMovieDetails } from "../../../api/movies"

export const getDetails = (id: number | string): AppThunk => {
  return async dispatch => {
    dispatch(startLoadingDetails())

    const { data }: { data: MovieDetailsResponse } = await getMovieDetails(id)

    const simpleMovieDetails: SimpleMovieDetails = {
      image: IMAGE_BASE_URL + data.poster_path,
      title: data.title,
      tagline: data.tagline,
      language: data.original_language,
      length: data.runtime,
      rate: data.vote_average,
      budget: data.budget,
      release_date: data.release_date,
      genres: data.genres,
      overview: data.overview,
    }

    dispatch(setDetails({ movieDetails: simpleMovieDetails }))
  }
}
