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
} from '../search-api';
import { Link, useNavigate, useLocation } from 'react-router-dom';

const StyledH1 = styled.h1`
  --f-h1: clamp(2rem, 1.6rem + 2vw, 3.2rem);

  margin-left: 0.5rem;
  margin-right: 0.5rem;
  position: relative;
  display: inline-block;
  font-size: var(--f-h1);
  line-height: var(--f-h1) * 1.2;
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
  const [searchQuery, setSearchQuery] = useState('');
  const [debouncedSearchQuery, setDebouncedSearchQuery] = useState(searchQuery);
  const [page, setPage] = useState(1);
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    // Check the URL for query parameters
    const params = new URLSearchParams(location.search);
    const query = params.get('searchQuery');
    const pageParam = params.get('page');

    if (query) {
      setSearchQuery(query);
      setDebouncedSearchQuery(query);
    }
    if (pageParam) {
      setPage(Number(pageParam));
    }
  }, [location.search]);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedSearchQuery(searchQuery);
      setPage(1);
    }, 500);

    return () => clearTimeout(handler);
  }, [searchQuery]);

  // Update the URL when searchQuery or page changes
  useEffect(() => {
    const queryParams = new URLSearchParams();
    if (searchQuery) queryParams.set('searchQuery', searchQuery);
    queryParams.set('page', page.toString());
    navigate(`?${queryParams.toString()}`, { replace: true });
  }, [searchQuery, page, navigate]);

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

  const handleReset = () => {
    setSearchQuery('');
    setDebouncedSearchQuery('');
    setPage(1);
    navigate('/');
  };

  return (
    <div>
      <PageSection>
        <Link to={'/'} onClick={handleReset}>
          <StyledH1>Movie Search</StyledH1>
        </Link>
        <SearchBar onSearchChange={setSearchQuery} value={searchQuery} />
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
