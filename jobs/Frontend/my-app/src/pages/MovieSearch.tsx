import { useState } from 'react';
import { PageSection } from '../components/PageSection';
import { Pagination } from '../components/Pagination';
import { SearchBar } from '../components/SearchBar';
import { MovieCard } from '../components/MovieCard';
import { ShowMoreCards } from '../components/ShowMoreCards';
import { useQuery } from '@tanstack/react-query';
import { fetchMovies } from '../search-api';

interface Movie {
  id: number;
  title: string;
  release_date: string;
  vote_average: string;
  poster_path: string;
}

export const MovieSearch = () => {
  const [page, setPage] = useState(1);

  const {
    data: movies,
    isLoading,
    isError,
  } = useQuery({
    queryFn: () => fetchMovies('potter', page * 10 - 10),
    queryKey: ['movies', page],
    keepPreviousData: true,
  });

  console.log(movies);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError || !movies) return <div>Error loading movies!</div>;

  return (
    <div>
      <PageSection>
        <h1>Movie Search</h1>
        <SearchBar />
      </PageSection>
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
    </div>
  );
};
