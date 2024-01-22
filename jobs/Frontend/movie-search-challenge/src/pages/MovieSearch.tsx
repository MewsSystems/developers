import { useEffect } from "react"
import { useAppDispatch, useAppSelector } from "../app/hooks"
import { getMovies } from "../store/slices/movies/thunks"
import Loader from "../components/loader/Loader"
import MovieList from "../components/movie-list/MovieList"

const MovieSearch = () => {
  const dispatch = useAppDispatch()
  const { isLoading, movies, page } = useAppSelector(state => state.movies)

  useEffect(() => {
    dispatch(getMovies())
  }, [dispatch])

  return (
    <>
      {isLoading ? (
        <Loader />
      ) : (
        <>
          <MovieList movies={movies} />

          {/* <button
            disabled={isLoading}
            onClick={() => dispatch(getMovies(page))}
          >
            Next
          </button> */}
        </>
      )}
    </>
  )
}

export default MovieSearch
