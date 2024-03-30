import { Box, Typography, CircularProgress } from "@mui/material";
import { useAppSelector } from "../../../redux/hooks";
import { moviesSelectors } from "../../../redux/movies/movies.slice.selectors";

export const MovieSearchLoading = () => {
  const hasSearchResults = useAppSelector(moviesSelectors.hasResults);
  const isLoading = useAppSelector(moviesSelectors.isLoading);

  if (isLoading && !hasSearchResults) {
    return (
      <Box
        display="flex"
        alignItems="center"
        justifyContent="center"
        pt={2}
        gap={2}
      >
        <Typography variant="h5">Loading</Typography>
        <CircularProgress />
      </Box>
    );
  }

  return null;
};
