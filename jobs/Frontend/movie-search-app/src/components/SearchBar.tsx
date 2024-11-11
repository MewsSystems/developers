import React, { useState } from 'react';
import styled from 'styled-components';

const Input = styled.input`
  padding: 10px;
  font-size: 16px;
  width: 100%;
`;

interface SearchBarProps {
  onSearch: (query: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ onSearch }) => {
  const [query, setQuery] = useState('');

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newQuery = e.target.value;
    setQuery(newQuery);
    onSearch(newQuery);
  };

  return <Input type="text" value={query} onChange={handleInputChange} placeholder="Search for movies..." />;
};

export default SearchBar;