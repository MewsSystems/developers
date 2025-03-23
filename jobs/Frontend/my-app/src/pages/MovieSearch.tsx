import { useState, useEffect } from 'react';
import { PageSection } from '../components/PageSection';
import { Pagination } from '../components/Pagination';
import { SearchBar } from '../components/SearchBar';
import { MovieCard } from '../components/MovieCard';
import { ShowMoreCards } from '../components/ShowMoreCards';
import { useQuery } from '@tanstack/react-query';
import { fetchMovies, MoviesData, fetchPopularMovies } from '../search-api';

interface Movie {
  id: number;
  title: string;
  release_date: string;
  vote_average: string;
  poster_path: string;
}

export const MovieSearch = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [debouncedSearchQuery, setDebouncedSearchQuery] = useState(searchQuery);
  const [page, setPage] = useState(1);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedSearchQuery(searchQuery);
      setPage(1);
    }, 500);

    return () => clearTimeout(handler);
  }, [searchQuery]);

  // Search movies query
  const {
    data: movies,
    isLoading: isMoviesLoading,
    isError: isMoviesError,
  } = useQuery<MoviesData>({
    queryFn: () => fetchMovies(debouncedSearchQuery, page * 10 - 10),
    queryKey: ['movies', debouncedSearchQuery, page],
    enabled: !!debouncedSearchQuery,
  });

  // Popular movies query
  const {
    data: popularMovies,
    isLoading: isPopularLoading,
    isError: isPopularError,
  } = useQuery<MoviesData>({
    queryFn: () => fetchPopularMovies(page * 10 - 10),
    queryKey: ['popularMovies', page],
    enabled: !debouncedSearchQuery,
  });

  const isLoading = debouncedSearchQuery ? isMoviesLoading : isPopularLoading;
  const isError = debouncedSearchQuery ? isMoviesError : isPopularError;
  const movieList = debouncedSearchQuery ? movies : popularMovies;

  return (
    <div>
      <PageSection>
        <h1>Movie Search</h1>
        <SearchBar onSearchChange={setSearchQuery} />
      </PageSection>

      {isLoading && <div>Loading...</div>}
      {isError && <div>Error loading movies!</div>}

      {movieList && (
        <>
          <PageSection direction="row">
            {movieList.movieArray.map((movie: Movie) => (
              <MovieCard
                key={movie.id}
                poster={movie.poster_path}
                name={movie.title}
                rating={movie.vote_average}
                release_date={movie.release_date}
                to={`/movie-detail/${movie.id}`}
              />
            ))}
          </PageSection>

          <PageSection>
            <ShowMoreCards />
            <Pagination
              currentPage={page}
              onPageChange={setPage}
              totalPages={movieList.totalPages}
            />
          </PageSection>
        </>
      )}
    </div>
  );
};
