import { useMovies } from '../../hooks/useMovies'

export const MovieList = () => {
  const { data, error, isLoading } = useMovies()

  console.log(data, error, isLoading)

  return (
    <div>
      <h1>Movie List</h1>
      <ul>HI MOVIES</ul>
    </div>
  )
}
