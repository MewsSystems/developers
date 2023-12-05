import { useState } from "react"
import styled from "styled-components"
import { useGetMoviesQuery } from "@/features/api/apiSlice"
import MovieCard, { MovieInterface } from "@/components/MovieCard"
import Input from "@/components/Input"

const SearchPageContainer = styled.div`
  display: flex;
  flex-direction: column;
`

function SearchPage() {
  const [searchTerm, setSearchTerm] = useState("")
  const {
    data: movies,
    isLoading,
    isSuccess,
    isError,
    error,
  } = useGetMoviesQuery({ term: searchTerm, page: 1 })

  return (
    <SearchPageContainer>
      <Input
        placeholder="Search a movie..."
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />
      <section>
        {isLoading && <p>Loading...</p>}
        {isSuccess && (
          <>
            <p>Total results: {movies.total_results}</p>
            <p>
              Page {movies.page} of {movies.total_pages}
            </p>
            {movies.results.map((movie: MovieInterface) => (
              <MovieCard key={movie.id} movie={movie} />
            ))}
          </>
        )}
        {isError && <p>An error ocurred: {error.toString()}</p>}
      </section>
    </SearchPageContainer>
  )
}

export default SearchPage
