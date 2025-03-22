import { useState } from 'react';
import styled from 'styled-components';

const StyledSearchBar = styled.input`
  max-width: 100%;
  width: 50vw;
  border: 1px solid lightgrey;
  border-radius: 10px;

  &[type='text']:hover,
  &[type='text']:focus {
    border-color: #007bff;
    outline: none;
    box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
  }
`;

interface SearchBarProps {
  onSearchChange: (query: string) => void;
}

export const SearchBar = ({ onSearchChange }: SearchBarProps) => {
  const [query, setQuery] = useState('');

  const handleChange = (value: string) => {
    setQuery(value);
    onSearchChange(value);
  };
  return (
    <>
      <label>
        <StyledSearchBar
          type="text"
          placeholder="Enter movie name or a keyword"
          value={query}
          onChange={(e) => handleChange(e.target.value)}
        />
      </label>
    </>
  );
};
