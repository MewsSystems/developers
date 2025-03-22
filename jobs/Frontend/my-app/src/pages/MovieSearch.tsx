import { useState, useEffect } from 'react';
import { PageSection } from '../components/PageSection';
import { Pagination } from '../components/Pagination';
import { SearchBar } from '../components/SearchBar';
import { MovieCard } from '../components/MovieCard';
import { ShowMoreCards } from '../components/ShowMoreCards';
import { useQuery } from '@tanstack/react-query';
import { fetchMovies, MoviesData } from '../search-api';

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
    return () => {
      clearTimeout(handler);
    };
  }, [searchQuery]);

  const {
    data: movies,
    isLoading,
    isError,
  } = useQuery<MoviesData>({
    queryFn: () => fetchMovies(debouncedSearchQuery, page * 10 - 10),
    queryKey: ['movies', debouncedSearchQuery, page],
    enabled: !!debouncedSearchQuery,
    keepPreviousData: true,
  });

  // console.log(movies);

  return (
    <div>
      <PageSection>
        <h1>Movie Search</h1>
        <SearchBar onSearchChange={setSearchQuery} />
      </PageSection>

      {isLoading && <div>Loading...</div>}
      {isError && <div>Error loading movies!</div>}
      {movies && (
        <>
          <PageSection direction="row">
            {movies?.movieArray.map((movie: Movie) => {
              return (
                <MovieCard
                  key={movie.id}
                  poster={movie.poster_path}
                  name={movie.title}
                  rating={movie.vote_average}
                  release_date={movie.release_date}
                />
              );
            })}
          </PageSection>

          <PageSection>
            <ShowMoreCards />
            <Pagination
              currentPage={page}
              onPageChange={setPage}
              totalPages={movies.totalPages}
            />
          </PageSection>
        </>
      )}
    </div>
  );
};
