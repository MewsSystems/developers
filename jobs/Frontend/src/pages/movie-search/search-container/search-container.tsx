import React from 'react';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import SearchBar from '../../../components/search-bar/search-bar';
import { StyledContainer } from './search-container.styled';
import { SearchContainerProps } from './search-container.interface';

const SearchContainer = ({ handleOnSearch }: SearchContainerProps) => {
  return (
    <Grid item xs={12}>
      <StyledContainer container alignItems="center">
        <Grid item xs={1} lg={3} />
        <Grid item xs={10} lg={6}>
          <Typography variant="h4" className="m-b-8">
            Discover Your Favorite Movies
          </Typography>
          <Typography variant="subtitle1" className="m-b-8">
            Search for any movie by title. Find detailed information for all
            your favorite films. Start exploring now!
          </Typography>
          <SearchBar onSearch={handleOnSearch} />
        </Grid>
      </StyledContainer>
    </Grid>
  );
};

export default SearchContainer;
