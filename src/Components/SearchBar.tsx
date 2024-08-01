import React from 'react';

interface SearchBarProps {
  searchInput: string;
  onSearchInputChange: (searchInput: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ searchInput, onSearchInputChange }) => {
  return (
    <input
      type="search"
      id="search"
      placeholder="Search for a movie..."
      value={searchInput}
      onChange={(e) => onSearchInputChange(e.target.value)}
    />
  );
};

export default SearchBar;
