import { QueryClientProvider } from '@tanstack/react-query'
import { queryClient } from '@/config/query'
import { MovieList } from '@/features/MovieList/MovieList'

export const Home = () => {
  return (
    <>
      <QueryClientProvider client={queryClient}>
        <MovieList />
      </QueryClientProvider>
    </>
  )
}
