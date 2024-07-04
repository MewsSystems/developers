import React, { memo } from "react";
import styled from "styled-components";
import SearchBar from "../components/SearchBar";
import MovieCardGrid from "../components/MovieCardGrid";
import Button from "../components/Button";
import LoadingSpinner from "../components/LoadingSpinner";
import PageContainer from "../components/PageContainer";
import { useSearchMovies } from "../hooks/movies";
import { SearchContext } from "../contexts/SearchContext";

const CenteredContent = styled.div`
  text-align: center;
`;

const MessageEmptyResult = styled.div`
  text-align: center;
  margin-top: 16px;
`;
const SearchPage: React.FC = memo(() => {
  const { searchTerms, setSearchTerms } = React.useContext(SearchContext);

  const { data, isLoading, isError, fetchNextPage, hasNextPage } =
    useSearchMovies(searchTerms);

  const handleSearch = (searchQuery: string) => {
    setSearchTerms(searchQuery);
  };

  const loadMore = () => {
    fetchNextPage();
  };

  const movies = data?.pages.flatMap((page) => page);

  return (
    <PageContainer>
      <SearchBar initialValue={searchTerms} onSearch={handleSearch} />
      {isError && <div>Error: Impossible to fetch the data</div>}
      {movies && <MovieCardGrid movies={movies} />}
      {isLoading && <LoadingSpinner />}
      {movies && movies.length > 0 && hasNextPage ? (
        <CenteredContent>
          <Button onClick={loadMore}>Load More</Button>
        </CenteredContent>
      ) : (
        <MessageEmptyResult>
          {searchTerms.length > 0
            ? "No results found."
            : "Please, type a movie title to start!"}
        </MessageEmptyResult>
      )}
    </PageContainer>
  );
});

export default SearchPage;
