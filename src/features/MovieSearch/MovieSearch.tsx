import { Skeleton } from '@/components/ui/skeleton'
import { useMovies } from '@/hooks/useMovies'
import { MovieCard } from './MovieCard'
import { useDebounceValue } from 'usehooks-ts'
import { Input } from '@/components/ui/input'
import { useState } from 'react'

const SEARCH_DEBOUNCE_DELAY = 500

export const MovieSearch = () => {
  const [searchValue, setSearchValue] = useState('')
  const [debouncedSearchValue] = useDebounceValue(
    searchValue,
    SEARCH_DEBOUNCE_DELAY,
  )
  const { data, error, isError, isPending } = useMovies(debouncedSearchValue, 1)

  console.log(data, error)

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchValue(e.target.value)
  }

  if (isError) {
    throw error
  }

  if (isPending) {
    return <Skeleton className="h-96" />
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold text-center mb-8">Movie Search</h1>
      <Input
        value={searchValue}
        onChange={handleSearchChange}
        type="text"
        placeholder="Search movie"
      />
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {data.results && data.results.length > 0 ? (
          data.results.map((movie) => (
            <MovieCard key={movie.id} movie={movie} />
          ))
        ) : (
          <div className="text-center text-2xl font-bold">No results found</div>
        )}
      </div>
    </div>
  )
}
