import { PageSectionContainer } from '../components/PageSectionContainer';
import { Pagination } from '../components/Pagination';
import { SearchBar } from '../components/SearchBar';
import { SearchResultCard } from '../components/SearchResultCard';
import { ShowMoreCards } from '../components/ShowMoreCards';

export const MovieSearch = () => {
  return (
    <div>
      <PageSectionContainer>
        <h1>Movie Search</h1>
        <SearchBar />
      </PageSectionContainer>
      <PageSectionContainer>
        <SearchResultCard />
      </PageSectionContainer>
      <ShowMoreCards />
      <Pagination />
    </div>
  );
};
