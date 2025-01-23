import Box from "@mui/material/Box";
import { useAppSelector } from "../../../redux/hooks";
import { moviesSelectors } from "../../../redux/movies/movies.slice.selectors";
import { MovieList } from "../../MovieList";
import { Typography } from "@mui/material";

export const SearchResults = () => {
  const query = useAppSelector(moviesSelectors.getQuery);
  const hasSearchResults = useAppSelector(moviesSelectors.hasResults);
  const isLoading = useAppSelector(moviesSelectors.isLoading);

  if (!hasSearchResults && !isLoading) {
    return null;
  }

  return (
    <Box>
      {hasSearchResults && (
        <>
          <Typography variant="h5" gutterBottom>
            Search results for: <b>{query}</b>
          </Typography>
          <MovieList />
        </>
      )}
    </Box>
  );
};
