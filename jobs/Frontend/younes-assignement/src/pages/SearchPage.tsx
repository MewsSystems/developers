import { useEffect } from "react";
import MovieCard from "../components/MovieCard";
import Loader from "../components/Loader";
import Layout from "../components/Layout";
import {
  LoadMoreButton,
  MovieSearchInput,
  MovieCardGrid,
} from "../styles/styles";
import useDebounce from "../hooks/useDebounce";
import useMovieSearch from "../hooks/useMovieSearch";
import EmptyState from "../components/EmptyState";
import { Film } from "lucide-react";
import ErrorState from "../components/ErrorState";

const SearchPage = () => {
  const { query, setQuery, movies, loading, error, page, search, reset } =
    useMovieSearch();

  const debouncedQuery = useDebounce(query, 500);

  useEffect(() => {
    if (debouncedQuery === "") {
      reset();
      return;
    }
    search(debouncedQuery);
  }, [debouncedQuery, search, reset]);

  const loadMore = () => search(debouncedQuery, page + 1);

  console.log(movies);

  return (
    <Layout>
      <h1>Movie Search</h1>
      <MovieSearchInput
        type="text"
        placeholder="Search movies..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        name="search"
      />
      {loading && <Loader />}
      {error && <ErrorState message={error} />}
      {!loading && movies.length === 0 && query !== "" && !error && (
        <EmptyState
          message="No movies found for your search."
          icon={<Film size={36} />}
        />
      )}
      <MovieCardGrid>
        {movies.map((movie) => (
          <MovieCard
            key={movie.id}
            id={movie.id}
            title={movie.title}
            poster_path={movie.poster_path}
            vote_average={movie.vote_average}
          />
        ))}
      </MovieCardGrid>
      {movies.length > 0 && !loading && (
        <LoadMoreButton onClick={loadMore} disabled={loading}>
          Load More
        </LoadMoreButton>
      )}
    </Layout>
  );
};

export default SearchPage;
