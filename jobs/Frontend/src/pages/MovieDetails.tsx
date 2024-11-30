import { Link, useParams } from 'react-router-dom'
import { useGetMovieDetails } from '../hooks/movies/useGetMovieDetails'

function MovieDetails() {
  const { id } = useParams() as { id: string }
  console.log(id)
  const {
    isLoading,
    isError,
    error,
    data: movieDetails,
  } = useGetMovieDetails(id)

  if (isLoading) return <div>Loading...</div>
  if (isError) return <div>Error: {error.message}</div>
  if (movieDetails)
    return (
      <main>
        {movieDetails.title}
        <Link to="/">Go back</Link>
      </main>
    )

  return (
    <main>
      <Link to="/">Go back</Link>
    </main>
  )
}

export default MovieDetails
