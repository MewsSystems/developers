import { useMovieDetail } from '@/hooks/useMovieDetail'
import { raiseError } from '@/lib/utils'
import { useParams } from '@tanstack/react-router'
import { MovieDetailCard } from '@/features/MovieDetail/MovieDetailCard'
import { Skeleton } from '@/components/ui/skeleton'

export function MovieDetail() {
  const { id } = useParams({ from: '/movie/$id' })

  const movieId = id ? id : raiseError('Movie ID is required')
  const { data: movie, isPending, isError, error } = useMovieDetail(movieId)

  if (isError) throw error

  return (
    <div className="max-w-4xl mx-auto p-6">
      {isPending ? (
        <Skeleton className="h-96" />
      ) : (
        <MovieDetailCard movie={movie} />
      )}
    </div>
  )
}

export default MovieDetail
