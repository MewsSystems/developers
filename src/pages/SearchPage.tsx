import { useState } from "react"
import styled from "styled-components"
import { ErrorMessage } from "../components/ErrorMessage"
import { LoadingSpinner } from "../components/LoadingSpinner"
import { MovieGrid } from "../components/MovieGrid"
import { SearchInput } from "../components/SearchInput"
import { useDebounce } from "../hooks/useDebounce"
import { useMovieSearch, usePopularMovies } from "../hooks/useMovies"

const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: ${({ theme }) => theme.spacing.xl};
`

const Title = styled.h1`
  font-size: ${({ theme }) => theme.fontSizes["3xl"]};
  margin-bottom: ${({ theme }) => theme.spacing.xl};
  text-align: center;
  color: ${({ theme }) => theme.colors.primary};
`

const SearchSection = styled.div`
  margin-bottom: ${({ theme }) => theme.spacing.xl};
`

const SectionTitle = styled.h2`
  font-size: ${({ theme }) => theme.fontSizes["2xl"]};
  margin-bottom: ${({ theme }) => theme.spacing.lg};
  color: ${({ theme }) => theme.colors.text};
  text-align: center;
`

export const SearchPage = () => {
  const [searchQuery, setSearchQuery] = useState("")
  const debouncedQuery = useDebounce(searchQuery, 500)

  const { data: popularData, isLoading: popularLoading, error: popularError } = usePopularMovies()

  const {
    data: searchData,
    isLoading: searchLoading,
    error: searchError,
  } = useMovieSearch(debouncedQuery)

  const handleSearchChange = (query: string) => {
    setSearchQuery(query)
  }

  const isSearching = !!debouncedQuery.trim()
  const data = isSearching ? searchData : popularData
  const isLoading = isSearching ? searchLoading : popularLoading
  const error = isSearching ? searchError : popularError

  return (
    <Container>
      <Title>🎬 Movie Search</Title>

      <SearchSection>
        <SearchInput
          value={searchQuery}
          onChange={handleSearchChange}
          placeholder="Search for movies..."
        />
      </SearchSection>

      {isLoading && <LoadingSpinner />}

      {error && (
        <ErrorMessage
          message={
            isSearching
              ? "Failed to search movies. Please try again."
              : "Failed to load popular movies. Please try again."
          }
        />
      )}

      {data && data.results.length > 0 && (
        <>
          <SectionTitle>
            {isSearching ? `Search Results for "${debouncedQuery}"` : "Popular Movies"}
          </SectionTitle>
          <MovieGrid movies={data.results} />
        </>
      )}

      {data && data.results.length === 0 && isSearching && (
        <ErrorMessage message="No movies found. Try a different search term." />
      )}
    </Container>
  )
}
