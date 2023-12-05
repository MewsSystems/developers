import { useState } from "react"
import styled from "styled-components"
import { useGetMoviesQuery } from "@/features/api/apiSlice"
import MovieCard, { MovieInterface } from "@/components/MovieCard"
import Input from "@/components/Input"
import Pagination from "@/components/Pagination"

const SearchPageContainer = styled.div`
  display: flex;
  flex-direction: column;
`

const SearchResultHeader = styled.div`
  margin: 1em 0em;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
`

function SearchPage() {
  const [searchTerm, setSearchTerm] = useState("")
  const [page, setPage] = useState(1)
  const {
    data: movies,
    isLoading,
    isSuccess,
    isError,
    error,
  } = useGetMoviesQuery({ term: searchTerm, page: page })

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
            <SearchResultHeader>
              <p>Total results: {movies.total_results}</p>
              <Pagination
                page={page}
                setPage={setPage}
                totalPages={movies.total_pages}
              />
            </SearchResultHeader>
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
