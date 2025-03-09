// MovieDetail.tsx
import { useMovieDetail } from '@/hooks/useMovieDetail'
import { raiseError } from '@/lib/utils'
import { useParams } from '@tanstack/react-router'

export function MovieDetail() {
  // Use TanStack Router's `useParams` to get the `id` from the URL
  const { id } = useParams({ from: '/movie/$id' })
  const movieId = id ? id : raiseError('Movie ID is required')
  const { data, isPending, isError, error } = useMovieDetail(movieId)

  if (isError) throw error
  if (isPending) return <div>Loading...</div>

  return (
    <div>
      <h1>Movie Details</h1>
      <pre>{JSON.stringify(data, null, 2)}</pre>
    </div>
  )
}
