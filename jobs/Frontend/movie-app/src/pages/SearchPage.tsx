import { useState } from "react"
import styled from "styled-components"
import { useSearchParams } from "react-router-dom"
import { useGetMoviesQuery } from "@/features/api/apiSlice"
import MovieCard, { MovieInterface } from "@/components/MovieCard"
import Input from "@/components/Input"
import Pagination from "@/components/Pagination"

const SearchPageContainer = styled.div`
  display: flex;
  flex-direction: column;
`

const SearchResultsContainer = styled.section`
  margin-top: 1em;
`

const SearchDetailsContainer = styled.div`
  margin-bottom: 1em;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
`

function SearchResultDetails({
  page,
  setPage,
  totalPages,
  totalMovies,
}: {
  page: number
  setPage: (_: number) => void
  totalPages: number
  totalMovies: number
}) {
  return (
    <SearchDetailsContainer>
      <p>Total results: {totalMovies}</p>
      <Pagination page={page} setPage={setPage} totalPages={totalPages} />
    </SearchDetailsContainer>
  )
}

function SearchPage() {
  const [searchParams, setSearchParams] = useSearchParams()
  const searchTerm = searchParams.get("q") || ""
  const [page, setPage] = useState(1)
  const {
    data: movies,
    isLoading,
    isSuccess,
    isFetching,
    isError,
    error,
  } = useGetMoviesQuery({ term: searchTerm, page: page })

  const updateSearchTerm = (searchTerm: string) => {
    if (searchTerm === "") {
      setSearchParams((params) => {
        params.delete("q")
        return params
      })
    } else {
      setSearchParams({ q: searchTerm })
    }
  }

  return (
    <SearchPageContainer>
      <Input
        type="search"
        placeholder="Search a movie..."
        value={searchTerm}
        onChange={(e) => updateSearchTerm(e.target.value)}
        aria-label="Search movies"
      />
      {searchTerm !== "" && (
        <SearchResultsContainer>
          {isLoading ||
            (isFetching && movies?.total_results === 0 && <p>Loading...</p>)}
          {isSuccess && movies?.results.length > 0 && (
            <>
              <SearchResultDetails
                page={page}
                setPage={setPage}
                totalPages={movies.total_pages}
                totalMovies={movies.total_results}
              />
              {movies.results.map((movie: MovieInterface) => (
                <MovieCard key={movie.id} movie={movie} />
              ))}
            </>
          )}
          {isSuccess && !isFetching && movies?.total_results === 0 && (
            <p>There are no results</p>
          )}
          {isError && <p>An error ocurred: {error.toString()}</p>}
        </SearchResultsContainer>
      )}
    </SearchPageContainer>
  )
}

export default SearchPage
