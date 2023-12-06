import { useEffect } from "react"
import styled from "styled-components"
import { useSearchParams } from "react-router-dom"
import { useGetMoviesQuery } from "@/features/api/apiSlice"
import MovieCard, { MovieInterface } from "@/components/MovieCard"
import Input from "@/components/Input"
import Pagination from "@/components/Pagination"
import ErrorCard from "@/components/ErrorCard"

interface MoviesListInterface {
  page: number
  total_results: number
  total_pages: number
  results: MovieInterface[]
}

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

function MoviesList({ movies }: { movies: MoviesListInterface }) {
  const {
    page,
    results: moviesList,
    total_results: totalEntries,
    total_pages: totalPages,
  } = movies

  const firstEntry = (page - 1) * 20 + 1
  const lastEntry = firstEntry + moviesList.length - 1
  return (
    <>
      <SearchDetailsContainer>
        <p>
          Showing {firstEntry} to {lastEntry} of {totalEntries} entries
        </p>
        <Pagination totalPages={totalPages} />
      </SearchDetailsContainer>
      {moviesList.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </>
  )
}

function SearchPage() {
  const [searchParams, setSearchParams] = useSearchParams()
  const searchTerm = searchParams.get("q") || ""
  const page = searchParams.get("p") ? Number(searchParams.get("p")) : 1
  const {
    data: movies,
    isLoading,
    isSuccess,
    isFetching,
    isError,
    error,
  } = useGetMoviesQuery({ term: searchTerm, page: page })

  useEffect(() => {
    if (page < 1 || page > movies?.total_pages) {
      setSearchParams((searchParams) => {
        searchParams.set("p", "1")
        return searchParams
      })
    }
  }, [movies, page, setSearchParams])

  const updateSearchTerm = (searchTerm: string) => {
    setSearchParams(searchTerm === "" ? {} : { q: searchTerm })
  }

  return (
    <SearchPageContainer>
      <Input
        type="search"
        name="q"
        placeholder="Search a movie..."
        value={searchTerm}
        onChange={(e) => updateSearchTerm(e.target.value)}
        aria-label="Search movies"
      />
      {searchTerm !== "" && (
        <SearchResultsContainer>
          {(isLoading || (isFetching && movies?.total_results === 0)) && (
            <p>Loading...</p>
          )}
          {isSuccess && movies?.results.length > 0 && (
            <MoviesList movies={movies} />
          )}
          {isSuccess && !isFetching && movies?.total_results === 0 && (
            <p>There are no results</p>
          )}
          {isError && <ErrorCard error={error} />}
        </SearchResultsContainer>
      )}
    </SearchPageContainer>
  )
}

export default SearchPage
