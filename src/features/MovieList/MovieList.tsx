import { Skeleton } from '@/components/ui/skeleton'
import { useMovies } from '../../hooks/useMovies'
import { MovieCard } from './MovieCard'

export const MovieList = () => {
  const { data, error, isError, isPending } = useMovies()

  console.log(data, error)

  if (isError) {
    throw error
  }

  if (isPending) {
    return <Skeleton className="h-96" />
  }

  return (
    <div>
      <h1>Movie List</h1>
      {data.results.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </div>
  )
}
