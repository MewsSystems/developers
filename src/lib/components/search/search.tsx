import styled from 'styled-components';
import { useDebouncedValue } from '../../useDebouncedValue';
import { useEffect, useState } from 'react';

const SearchContainer = styled.div`
  width: 100%;
  max-width: 600px;
  margin: 0 auto 2rem;
`;

const SearchInput = styled.input`
  width: 100%;
  padding: 1rem;
  font-size: 1.1rem;
  border: 2px solid #eee;
  border-radius: 8px;
  transition: border-color 0.2s ease;

  &:focus {
    outline: none;
    border-color: #007bff;
  }

  &::placeholder {
    color: #999;
  }
`;

interface SearchProps {
  onSearch: (query: string) => void;
  placeholder?: string;
}

export const Search: React.FC<SearchProps> = ({ 
  onSearch, 
  placeholder = 'Search movies...' 
}) => {
  const [value, setValue] = useState('');
  const debouncedValue = useDebouncedValue(value, 500);
  

  useEffect(() => {
    onSearch(debouncedValue);
  }, [debouncedValue, onSearch]);

  return (
    <SearchContainer>
      <SearchInput
        type="text"
        value={value}
        onChange={(e) => setValue(e.target.value)}
        placeholder={placeholder}
      />
    </SearchContainer>
  );
}; 