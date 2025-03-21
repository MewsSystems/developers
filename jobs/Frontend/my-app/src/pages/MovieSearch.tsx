import { PageSectionContainer } from '../components/PageSectionContainer';
import { Pagination } from '../components/Pagination';
import { SearchBar } from '../components/SearchBar';
import { MovieCard } from '../components/MovieCard';
import { ShowMoreCards } from '../components/ShowMoreCards';
import { useQuery } from '@tanstack/react-query';
import { fetchData } from '../search-api';

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
      <PageSectionContainer>
        <h1>Movie Search</h1>
        <SearchBar />
      </PageSectionContainer>
      <PageSectionContainer>
        {movies.results?.map((movie) => {
          return <MovieCard key={movie.id} />;
        })}
      </PageSectionContainer>
      <ShowMoreCards />
      <Pagination />
    </div>
  );
};
