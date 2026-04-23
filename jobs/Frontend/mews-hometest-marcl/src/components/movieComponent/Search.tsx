import React, { useState } from 'react';
import { Dispatch } from 'redux';

import { TextField, Box, Paper, InputAdornment, IconButton } from '@mui/material';
import { Clear as ClearIcon } from '@mui/icons-material';
import { useDispatch, useSelector } from 'react-redux';
import { setSearchText } from '../../store/actions/movies.actions';
import '../../styles/Search.css';

interface SearchBoxProps {
    onSearch: (searchText: string) => void;
  }

const Search: React.FC<SearchBoxProps> = ({ onSearch }) => {

    const [isFocused, setIsFocused] = useState(false);
    const dispatch: Dispatch<any> = useDispatch();
    const searchText = useSelector((state: any) => state.movies.searchText);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const text = e.target.value;
      dispatch(setSearchText(text));
      onSearch(text);
    };

    const handleFocus = () => {
      setIsFocused(true);
    };

    const handleBlur = () => {
      setTimeout(() => {
        setIsFocused(false);
      }, 100); // Added to enable the handleClear before the setIsFocused occurs
    };

    const handleClear = () => {
      dispatch(setSearchText(''));
      onSearch('');
    };

    return (
        <Box className="search-container" marginBottom={2}>
          <Paper
            elevation={3}
            className="search-paper"
            style={{ marginTop: isFocused ? 0 : '40vh' }}
          >
            <TextField
              id="search"
              label="Search movies"
              variant="filled"
              value={searchText}
              onChange={handleInputChange}
              fullWidth
              onFocus={handleFocus}
              onBlur={handleBlur}
              InputProps={{ 
                disableUnderline: true, 
                className:"search-input", 
                endAdornment: (
                  <InputAdornment position="end">
                    {searchText && (
                      <IconButton onClick={handleClear}>
                        <ClearIcon />
                      </IconButton>
                    )}
                  </InputAdornment>
                ),
              }}
            />
          </Paper>
        </Box>
      );

};

export default Search;
