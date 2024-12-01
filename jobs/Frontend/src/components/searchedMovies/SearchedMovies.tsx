import { useState } from 'react'
import { useGetMovies } from '../../hooks/movies/useGetMovies'
import MovieList from '../movieList/MovieList'
import Pagination from '../pagination/Pagination'
import Card from '../ui/card/Card'
import { styled } from 'styled-components'

type SearchedMoviesProps = {
  query: string
}

function SearchedMovies({ query }: SearchedMoviesProps) {
  const [page, setPage] = useState(1)
  const { isLoading, isError, error, data } = useGetMovies(query, page)

  const totalPages = data?.total_pages ? data.total_pages : 1

  function renderPagination() {
    if (data && data.total_pages > 1) {
      return (
        <Pagination
          currentPage={page}
          setCurrentPage={setPage}
          totalPages={totalPages}
        />
      )
    }
  }

  if (isLoading) {
    return (
      <SearchedMoviesGrid>
        {Array.from({ length: 20 }).map((_, i) => {
          return <Card key={i} isSkeleton />
        })}
      </SearchedMoviesGrid>
    )
  }
  if (isError) return <div className="error">{error.message}</div>

  if (data) {
    return (
      <>
        {renderPagination()}
        <SearchedMoviesGrid>
          <MovieList movies={data.results} />
        </SearchedMoviesGrid>
        {renderPagination()}
      </>
    )
  }
}

const SearchedMoviesGrid = styled.div`
  flex-grow: 1;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1rem;
`

export default SearchedMovies
