import { PageSectionContainer } from '../components/PageSectionContainer';
import { Pagination } from '../components/Pagination';
import { SearchBar } from '../components/SearchBar';
import { MovieCard } from '../components/MovieCard';
import { ShowMoreCards } from '../components/ShowMoreCards';

export const MovieSearch = () => {
  return (
    <div>
      <PageSectionContainer>
        <h1>Movie Search</h1>
        <SearchBar />
      </PageSectionContainer>
      <PageSectionContainer>
        <MovieCard />
      </PageSectionContainer>
      <ShowMoreCards />
      <Pagination />
    </div>
  );
};
