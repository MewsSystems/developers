import { styled } from 'styled-components'
import { useGetMovies } from '../hooks/movies/useGetMovies'
import { useState } from 'react'
import { useDebounce } from '../hooks/utils/useDebounce'

import Card from '../components/ui/card/Card'
import SearchBar from '../components/ui/searchBar/SearchBar'
import MovieList from '../components/movieList/MovieList'
import Pagination from '../components/pagination/Pagination'

function Home() {
  const [searchQuery, setSearchQuery] = useState('')
  const [currentPage, setCurrentPage] = useState(1)

  const debouncedSearchQuery = useDebounce(searchQuery, 500)
  console.log(debouncedSearchQuery)

  const { isLoading, isError, error, data } = useGetMovies(
    debouncedSearchQuery,
    currentPage
  )

  const totalPages = data?.total_pages ? data.total_pages : 1

  function renderMovies() {
    if (isLoading) {
      return (
        <>
          {Array.from({ length: 20 }).map((_, i) => {
            return <Card key={i} isSkeleton />
          })}
        </>
      )
    }
    if (isError) return <div className="error">{error.message}</div>
    if (data) return <MovieList movies={data.results} />
  }

  function renderPagination() {
    if (data && data.total_pages > 1) {
      return (
        <Pagination
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
          totalPages={totalPages}
        />
      )
    }
  }

  return (
    <StyledHome id="home">
      <div className="floating-search">
        <SearchBar
          searchQuery={searchQuery}
          setSearchQuery={setSearchQuery}
          placeholder="Search for a movie"
          autoFocus
        />
      </div>
      {renderPagination()}
      <section className="home-layout">{renderMovies()}</section>
      {renderPagination()}
    </StyledHome>
  )
}

const StyledHome = styled.main`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  margin: 1rem;
  gap: 1rem;

  .floating-search {
    position: absolute;
    top: 122px;
  }

  .home-layout {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 1rem;
  }
`

export default Home
