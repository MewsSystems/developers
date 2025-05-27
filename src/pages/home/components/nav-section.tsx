import { Search } from '@app/lib/components/search/search';
import styled from 'styled-components';

interface NavSectionProps {
  setSearchQuery: (query: string) => void;
}

const SearchContainer = styled.div`
  width: 100%;
  max-width: 600px;
  display: flex;
  gap: 1rem;
  align-items: center;
`;

const SearchWrapper = styled.div`
  flex: 1;
  max-width: 600px;
  margin: 0 auto;

  @media (max-width: 768px) {
    width: 100%;
  }
`;

export const NavSection = ({ setSearchQuery }: NavSectionProps) => {
  const handleSearch = (query: string) => {
    setSearchQuery(query);
  };

  return (
    <SearchWrapper>
      <SearchContainer>
        <Search onSearch={handleSearch} />
      </SearchContainer>
    </SearchWrapper>
  );
};
