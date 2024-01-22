import { useParams } from "react-router-dom"
import MovieDetails from "../components/movie-details/MovieDetails"
import { useAppDispatch, useAppSelector } from "../app/hooks"
import { getDetails } from "../app/slices/movieDetails/thunks"
import { useEffect } from "react"
import Loader from "../components/loader/Loader"

const MovieInfo = () => {
  const { id } = useParams()
  const dispatch = useAppDispatch()
  const { isLoading, details } = useAppSelector(state => state.movieDetails)

  useEffect(() => {
    dispatch(getDetails(id!))
  }, [dispatch, id])

  return isLoading ? <Loader /> : <MovieDetails movieDetails={details} />
}
export default MovieInfo
