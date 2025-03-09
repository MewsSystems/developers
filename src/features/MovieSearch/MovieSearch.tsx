import { Skeleton } from '@/components/ui/skeleton'
import { useMovies } from '../../hooks/useMovies'
import { MovieCard } from './MovieCard'

export const MovieSearch = () => {
  const { data, error, isError, isPending } = useMovies('batman', 1)

  console.log(data, error)

  if (isError) {
    throw error
  }

  if (isPending) {
    return <Skeleton className="h-96" />
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8 text-center">Movie Search</h1>

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {data.results.map((movie) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </div>
    </div>
  )
}
