import { FC } from 'react';
import styled from 'styled-components';

const SearchWrapper = styled.div<{ isActive: boolean }>`
  width: 100%;
  max-width: 600px;
  margin-top: ${({ isActive }) => (isActive ? '1rem' : '40vh')};
  transition: margin 0.3s ease;
`;

const Input = styled.input`
  width: 100%;
  padding: 1rem;
  font-size: 1.25rem;
  border-radius: 8px;
  border: 1px solid #ccc;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
`;

type Props = {
  query: string;
  setQuery: (val: string) => void;
};

const SearchBar: FC<Props> = ({ query, setQuery }) => {
  return (
    <SearchWrapper isActive={!!query}>
      <Input
        type="text"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        placeholder="Start typing to find a movie"
      />
    </SearchWrapper>
  );
};

export default SearchBar;
