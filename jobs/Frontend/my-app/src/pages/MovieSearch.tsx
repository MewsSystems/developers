import { PageSection } from '../components/PageSection';
import { Pagination } from '../components/Pagination';
import { SearchBar } from '../components/SearchBar';
import { MovieCard } from '../components/MovieCard';
import { ShowMoreCards } from '../components/ShowMoreCards';
import { useQuery } from '@tanstack/react-query';
import { fetchData } from '../search-api';

interface Movie {
  id: number;
  title: string;
  release_date: string;
  vote_average: string;
  poster_path: string;
}

export const MovieSearch = () => {
  const { data: movies, isLoading } = useQuery({
    queryFn: () => fetchData('potter'),
    queryKey: ['movies'],
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }
  return (
    <div>
      <PageSection>
        <h1>Movie Search</h1>
        <SearchBar />
      </PageSection>
      <PageSection direction="row">
        {movies.results?.map((movie: Movie) => {
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
      <ShowMoreCards />
      <Pagination />
    </div>
  );
};
