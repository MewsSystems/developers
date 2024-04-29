import React, { ChangeEvent } from 'react';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputAdornment from '@mui/material/InputAdornment';
import Icon from '@mui/material/Icon';
import { SearchBarProps } from './search-bar.interface';

const SearchBar = ({ searchTerm, onSearch }: SearchBarProps) => {
  const handleSearch = (event: ChangeEvent<HTMLInputElement>) => {
    onSearch(event.target.value);
  };

  return (
    <OutlinedInput
      onChange={handleSearch}
      placeholder="Search..."
      style={{ backgroundColor: 'white' }}
      value={searchTerm ? searchTerm : ''}
      endAdornment={
        <InputAdornment position="end">
          <Icon>search</Icon>
        </InputAdornment>
      }
      fullWidth
    />
  );
};

export default SearchBar;
