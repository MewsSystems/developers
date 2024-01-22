import { useEffect } from "react"
import { useAppDispatch, useAppSelector } from "../app/hooks"
import { getMovies } from "../app/slices/movieList/thunks"
import Loader from "../components/loader/Loader"
import MovieList from "../components/movie-list/MovieList"
import Header from "../components/header/Header"

const MovieSearch = () => {
  const dispatch = useAppDispatch()
  const { isLoading, movies } = useAppSelector(state => state.movieList)

  useEffect(() => {
    dispatch(getMovies())
  }, [dispatch])

  return (
    <>
      <Header />
      {isLoading ? <Loader /> : <MovieList movies={movies} />}
    </>
  )
}

export default MovieSearch
