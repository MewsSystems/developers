import { useState } from "react"
import styled from "styled-components"
import { ErrorMessage } from "../components/ErrorMessage"
import { MovieGrid } from "../components/MovieGrid"
import { Pagination } from "../components/Pagination"
import { SearchInput } from "../components/SearchInput"
import { useDebounce } from "../hooks/useDebounce"
import { useMovieSearch, usePopularMovies } from "../hooks/useMovies"
import { usePagination } from "../hooks/usePagination"

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

const ResultsInfo = styled.p`
  text-align: center;
  color: ${({ theme }) => theme.colors.textSecondary};
  margin-bottom: ${({ theme }) => theme.spacing.lg};
  font-size: ${({ theme }) => theme.fontSizes.sm};
`

export const SearchPage = () => {
  const [searchQuery, setSearchQuery] = useState("")
  const debouncedQuery = useDebounce(searchQuery, 500)

  const [popularPage, setPopularPage] = useState(1)
  const [searchPage, setSearchPage] = useState(1)

  const {
    data: popularData,
    isLoading: popularLoading,
    error: popularError,
  } = usePopularMovies(popularPage)

  const {
    data: searchData,
    isLoading: searchLoading,
    error: searchError,
  } = useMovieSearch(debouncedQuery, searchPage)

  const handleSearchChange = (query: string) => {
    setSearchQuery(query)
    setSearchPage(1)
  }

  const isSearching = !!debouncedQuery.trim()
  const data = isSearching ? searchData : popularData
  const isLoading = isSearching ? searchLoading : popularLoading
  const error = isSearching ? searchError : popularError

  const totalPages = data?.total_pages || 0
  const currentPage = isSearching ? searchPage : popularPage

  const handlePageChange = (page: number) => {
    if (isSearching) {
      setSearchPage(page)
    } else {
      setPopularPage(page)
    }

    window.scrollTo({ top: 0, behavior: "smooth" })
  }

  const pagination = usePagination({
    totalPages,
    currentPage,
    maxVisiblePages: 3,
    onPageChange: handlePageChange,
  })

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

      {error && (
        <ErrorMessage
          message={
            isSearching
              ? "Failed to search movies. Please try again."
              : "Failed to load popular movies. Please try again."
          }
        />
      )}

      {isLoading && (
        <>
          <SectionTitle>{isSearching ? "Searching..." : "Loading Popular Movies..."}</SectionTitle>
          <MovieGrid isLoading={true} />
        </>
      )}

      {data && data.results.length > 0 && !isLoading && (
        <>
          <SectionTitle>
            {isSearching ? `Search Results for "${debouncedQuery}"` : "Popular Movies"}
          </SectionTitle>

          {data.total_results && (
            <ResultsInfo>
              {isSearching
                ? `Found ${data.total_results.toLocaleString()} movies`
                : `${data.total_results.toLocaleString()} popular movies`}
              {totalPages > 1 && ` • Page ${currentPage} of ${totalPages}`}
            </ResultsInfo>
          )}

          <MovieGrid movies={data.results} />

          {totalPages > 1 && <Pagination {...pagination} />}
        </>
      )}

      {data && data.results.length === 0 && isSearching && (
        <ErrorMessage message="No movies found. Try a different search term." />
      )}
    </Container>
  )
}
