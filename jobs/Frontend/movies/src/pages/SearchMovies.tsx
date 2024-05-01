import useMoviesQuery from "src/hooks/useMoviesQuery";
import useDebouncedState from "src/hooks/useDebouncedSetState";
import MovieCard from "src/components/MovieCard";

function SearchMovies({}) {
  const [query, setQuery, isQueryChanging] =  useDebouncedState("", 1000); 

  const {
    movies,
    isLoading,
    isError,
    error,
    isFetching,
    isFetchingNextPage,
    hasNextPage,
    fetchNextPage,
  } = useMoviesQuery(query);

  return (
    <>
      <h1>Search movies</h1>
      {isLoading && <p>Loading</p>}
      {isFetching && <p>Fetching</p>}
      {isFetchingNextPage && <p>Fetching next page</p>}
      {isError && <p>Error occured</p>}
      {error && error instanceof Error && <p>{error.message}</p>}

      <label>
        Find movies:
        <input
          name="queryText"
          defaultValue={query}
          onChange={(e)=> setQuery(e.target.value)}
        />
      </label>

      <button
        name="loadMore"
        onClick={() => fetchNextPage()}
        disabled={isQueryChanging || isFetching || !hasNextPage}
      >
        Load more
      </button>

      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </>
  );
}

export default SearchMovies;
