import { useEffect } from "react"
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
            <>
              <SearchDetailsContainer>
                <p>Total results: {movies.total_results}</p>
                <Pagination totalPages={movies.total_pages} />
              </SearchDetailsContainer>
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
