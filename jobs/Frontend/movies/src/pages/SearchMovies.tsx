import useMoviesQuery from "src/hooks/useMoviesQuery";
import useDebouncedState from "src/hooks/useDebouncedSetState";
import MovieCard from "src/components/MovieCard";
import styled from "styled-components";
import Spinner from "src/components/Spinner";

function SearchMovies({}) {
  const [query, setQuery, isQueryChanging] = useDebouncedState("", 1000);

  const { movies, isError, error, isFetching, hasNextPage, fetchNextPage } =
    useMoviesQuery(query);

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
      {isFetching && <Spinner />}
      <>
        {isError && <p>Error occured</p>}
        {error && error instanceof Error && <p>{error.message}</p>}
      </>
      <MoviesSection>
        {movies.map((movie) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </MoviesSection>
      <button
        name="loadMore"
        onClick={() => fetchNextPage()}
        disabled={isQueryChanging || isFetching || !hasNextPage}
      >
        Load more
      </button>
    </Wrapper>
  );
}

const Wrapper = styled.div`
  display: flex;
  flex-direction: column;
  gap: 32px;
`;

const SearchBar = styled.section`
  padding: 16px 16px;
  background-color: rgba(255, 255, 255, 0.88);
  backdrop-filter: blur(8px);
  box-shadow: 0 10px 10px -5px rgba(200, 200, 200, 0.2);
  background-color: white;
  margin-left: -50vw;
  margin-right: -50vw;
`;

const Input = styled.input`
  padding: 8px 16px;
  border-radius: 8px;
  border: 3px solid black;
  width: 25vw;

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

export default SearchMovies;
