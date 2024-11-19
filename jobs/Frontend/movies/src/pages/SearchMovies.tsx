import useMoviesQuery from "src/hooks/useMoviesQuery";
import useDebouncedState from "src/hooks/useDebouncedSetState";
import { useEffect } from "react";
import { Link, useSearch } from "wouter";
import MovieCard from "src/components/MovieCard";
import styled from "styled-components";
import Spinner from "src/components/Spinner";
import AttributionNotice from "src/components/AttributionNotice";

function SearchMovies({}) {
  const search = useSearch();
  const queryParam = new URLSearchParams(search).get("query") || "";

  const [query, setQuery, isQueryChanging] = useDebouncedState(
    queryParam || "",
    250
  );

  const { movies, isError, error, isFetching, hasNextPage, fetchNextPage } =
    useMoviesQuery(query);

  // update url param when query changes with just replacing history
  useEffect(() => {
    if (queryParam != query) {
      window.history.pushState(null, "", `?${new URLSearchParams({ query })}`);
    }
  }, [query]);

  return (
    <Wrapper>
      <SearchBar>
        <Input
          type="search"
          placeholder="Search movies..."
          aria-label="Search movies"
          defaultValue={query}
          onChange={(e) => setQuery(e.target.value)}
        />
      </SearchBar>
      <AttributionNotice />
      {isFetching && <Spinner />}

      <>
        {isError && <p>Error occured</p>}
        {error && error instanceof Error && <p>{error.message}</p>}
      </>
      
      <MoviesSection>
        {movies.map((movie) => (
          <Link key={movie.id} href={`/movies/${movie.id}`}>
            <MovieCard movie={movie} />
          </Link>
        ))}
      </MoviesSection>

      {hasNextPage && (
        <LoadMoreButton
          onClick={() => fetchNextPage()}
          disabled={isQueryChanging || isFetching || !hasNextPage}
        >
          Load more
        </LoadMoreButton>
      )}      
    </Wrapper>
  );
}

const Wrapper = styled.div`
  display: flex;
  flex-direction: column;
  gap: 32px;
  padding-bottom: 32px;
`;

const SearchBar = styled.section`
  display: flex;
  justify-content: center;
  padding: 16px 16px;
  background-color: rgba(255, 255, 255, 0.88);
  backdrop-filter: blur(8px);
  box-shadow: 0 10px 10px -5px rgba(200, 200, 200, 0.2);
  background-color: white;
`;

const Input = styled.input`
  padding: 8px 16px;
  border-radius: 8px;
  border: 3px solid black;
  flex: 0 1 50vw;

  &:focus {
    outline: 1px solid black;
    outline-offset: 0px;
  }
`;

const MoviesSection = styled.section`
  display: flex;
  flex-wrap: wrap;
  gap: 30px;
  align-items: baseline;
  justify-content: center;
`;

const LoadMoreButton = styled.button`
  align-self: center;
  width: fit-content;
  background-color: var(--color-light-gray);
  color: black;
  border: 3px solid black;
  border-radius: 3px;
  padding: 8px 32px;
  font-size: 0.75rem;
  cursor: pointer;

  &:focus,
  &:hover {
    outline: 1px solid black;
    outline-offset: 0px;
  }
`;

export default SearchMovies;
