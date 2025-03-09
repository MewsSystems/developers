import { Skeleton } from '@/components/ui/skeleton'
import { useMovies } from '@/hooks/useMovies'
import { useDebounceValue } from 'usehooks-ts'
import { Input } from '@/components/ui/input'
import { SearchResult } from './SearchResult'
import { useState } from 'react'
import { Pagination } from './Pagination'
import { useNavigate, useSearch } from '@tanstack/react-router' // Import TanStack Router hooks
import { SEARCH_DEBOUNCE_DELAY } from '@/lib/utils'

export const MovieSearch = () => {
  const navigate = useNavigate()
  const searchParameters = useSearch({ from: '/' })

  const [searchValue, setSearchValue] = useState(searchParameters.search ?? '')
  const [page, setPage] = useState(Number(searchParameters.page ?? 1))

  const [debouncedSearchValue] = useDebounceValue(
    searchValue,
    SEARCH_DEBOUNCE_DELAY,
  )

  const { data, error, isError, isPending } = useMovies(
    debouncedSearchValue,
    page,
  )

  const isFirstSearch = debouncedSearchValue.trim().length === 0

  const handlePageChange = (newPage: number) => {
    setPage(newPage)
    navigate({ to: '/', search: { ...searchParameters, page: newPage } })
  }

  const handleSearchValueChange = (value: string) => {
    setSearchValue(value)
    navigate({ to: '/', search: { search: value, page: 1 } })
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
          onChange={(e) => handleSearchValueChange(e.target.value)}
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
        <>
          <SearchResult movies={data?.results} />
          {data?.results.length > 0 && (
            <Pagination
              page={page}
              setPage={handlePageChange} // Use handlePageChange here
              totalPages={data.total_pages}
            />
          )}
        </>
      )}
    </div>
  )
}
