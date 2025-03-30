import styled from 'styled-components';
import { PageSection } from '../components/PageSection';
import { Pagination } from '../components/Pagination/Pagination';
import { SearchBar } from '../components/SearchBar';
import { MovieCard } from '../components/MovieCard';
import { useState, useEffect } from 'react';
import { useQuery } from '@tanstack/react-query';
import {
  fetchPopularMovies,
  fetchMovies,
  Movie,
  MoviesData,
} from '../search-api.tsx';
import { useSearchParams } from 'react-router-dom';
import { Link, useNavigate } from 'react-router-dom';

const StyledH1 = styled.h1`
  --f-h1: clamp(2rem, 1.6rem + 2vw, 3.2rem);

  margin-left: 0.5rem;
  margin-right: 0.5rem;
  position: relative;
  display: inline-block;
  font-size: var(--f-h1);
  line-height: calc(var(--f-h1) * 1.2);
  color: #141414;

  &::before {
    content: '';
    position: absolute;
    left: 0;
    bottom: 0;
    height: 3px;
    width: 0;
    background: linear-gradient(to right, #f43f5e, #c026d3, #4d5b9e);
    transition: all 0.3s ease;
  }

  &:hover::before {
    width: 100%;
  }
`;

export const MovieSearch = () => {
  const [searchParams, setSearchParams] = useSearchParams('');
  const [query, setQuery] = useState(searchParams.get('query') || '');
  const [debouncedQuery, setDebouncedQuery] = useState('');
  const [page, setPage] = useState(Number(searchParams.get('page') || 1));
  const navigate = useNavigate();

  // Update the URL when searchQuery or page changes
  useEffect(() => {
    setSearchParams({ query: query, page: page.toString() });
  }, [query, page, setSearchParams]);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedQuery(query);
    }, 500);
    return () => clearTimeout(handler);
  }, [query]);

  // Popular movies query
  const {
    data: popularMovies,
    isLoading: isPopularLoading,
    isError: isPopularError,
  } = useQuery<MoviesData>({
    queryFn: () => fetchPopularMovies(page * 10 - 10),
    queryKey: ['popularMovies', page],
    enabled: !debouncedQuery,
  });

  // Search movies query
  const {
    data: movies,
    isLoading: isMoviesLoading,
    isError: isMoviesError,
  } = useQuery<MoviesData>({
    queryFn: () => fetchMovies(debouncedQuery, page * 10 - 10),
    queryKey: ['movies', debouncedQuery, page],
    enabled: !!debouncedQuery,
  });

  const handleReset = () => {
    setQuery('');
    setPage(1);
    navigate('/');
  };

  const isLoading = debouncedQuery ? isMoviesLoading : isPopularLoading;
  const isError = debouncedQuery ? isMoviesError : isPopularError;
  const movieList = debouncedQuery ? movies : popularMovies;

  return (
    <div>
      <PageSection>
        <Link to={'/'} onClick={handleReset}>
          <StyledH1>Movie Search</StyledH1>
        </Link>
        <SearchBar onSearchChange={setQuery} value={query} />
      </PageSection>

      {isLoading && <div>Loading...</div>}
      {isError && <div>Error loading movies!</div>}
      {movieList && movieList.movieArray.length === 0 && (
        <div>No movies found.</div>
      )}

      {movieList && movieList.movieArray.length > 0 && (
        <>
          <PageSection direction="row">
            {movieList.movieArray.map((movie: Movie) => (
              <MovieCard
                key={movie.id}
                poster={movie.poster_path}
                name={movie.title}
                rating={movie.vote_average}
                release_date={movie.release_date}
                to={`movie-detail/${movie.id}`}
              />
            ))}
          </PageSection>

          <PageSection>
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
