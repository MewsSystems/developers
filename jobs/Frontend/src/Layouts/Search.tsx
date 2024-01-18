import Grid from '@/components/Grid';
import Box from '@/components/Box';
import SearchInput from '@/components/SearchInput';
import MovieList from '@/components/MovieList';

const SearchLayout = () => {
  return (
    <Grid
      gridGap="16px"
      gridTemplateRows="auto 1fr"
    >
      <Box>
        <SearchInput/>
      </Box>
        <MovieList/>
    </Grid>
  )
}

export default SearchLayout;
