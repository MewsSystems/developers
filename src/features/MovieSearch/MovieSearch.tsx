import { Skeleton } from '@/components/ui/skeleton'
import { useMovies } from '@/hooks/useMovies'
import { useDebounceValue } from 'usehooks-ts'
import { Input } from '@/components/ui/input'
import { useState } from 'react'
import { SearchResult } from './SearchResult'

const SEARCH_DEBOUNCE_DELAY = 500

export const MovieSearch = () => {
  const [searchValue, setSearchValue] = useState('')
  const [debouncedSearchValue] = useDebounceValue(
    searchValue,
    SEARCH_DEBOUNCE_DELAY,
  )
  const { data, error, isError, isPending } = useMovies(debouncedSearchValue, 1)

  const isFirstSearch = debouncedSearchValue.trim().length === 0

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchValue(e.target.value)
  }

  if (isError) {
    throw error
  }

  return (
    <div className="w-full mx-auto px-4 py-8 min-h-screen">
      <header className="mb-8 flex flex-col items-center">
        <h1 className="text-3xl font-bold text-center mb-8">
          TMDB Movie Search
        </h1>
        <Input
          value={searchValue}
          onChange={handleSearchChange}
          type="text"
          placeholder="Search movie"
          autoFocus
          className="w-2/3"
        />
      </header>
      {isPending ? (
        <Skeleton className="h-96" />
      ) : isFirstSearch ? (
        <div className="flex justify-center items-center mt-8 text-center text-xl text-gray-600">
          Enter a movie title to search
        </div>
      ) : (
        <SearchResult movies={data?.results} />
      )}
    </div>
  )
}
