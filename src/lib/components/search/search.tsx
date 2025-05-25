import styled from 'styled-components';
import { useDebouncedValue } from '@app/lib/useDebouncedValue';
import { useEffect, useState } from 'react';

const SearchContainer = styled.div`
  width: 100%;
  max-width: 600px;
  margin: 0 auto 2rem;
  display: flex;
  gap: 1rem;
  align-items: center;
`;

const SearchInput = styled.input`
  flex: 1;
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

const ResetButton = styled.button`
  padding: 1rem;
  background: #f8f9fa;
  border: 2px solid #eee;
  border-radius: 8px;
  color: #666;
  cursor: pointer;
  transition: all 0.2s ease;
  white-space: nowrap;

  &:hover {
    background: #e9ecef;
    border-color: #dee2e6;
  }

  &:active {
    background: #dee2e6;
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

  const handleReset = () => {
    setValue('');
    onSearch('');
  };

  return (
    <SearchContainer>
      <SearchInput
        type="text"
        value={value}
        onChange={(e) => setValue(e.target.value)}
        placeholder={placeholder}
      />
      {value && (
        <ResetButton onClick={handleReset}>
          Reset
        </ResetButton>
      )}
    </SearchContainer>
  );
}; 