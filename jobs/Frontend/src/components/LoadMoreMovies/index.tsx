import { Box, Button } from "@mui/material";
import { FC } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import { moviesSelectors } from "../../redux/movies/movies.slice.selectors";
import { moviesThunks } from "../../redux/movies/movies.slice.thunks";

export const LoadMoreMovies: FC = () => {
  const dispatch = useAppDispatch();
  const isLoading = useAppSelector(moviesSelectors.isLoading);
  const page = useAppSelector(moviesSelectors.getSearchPage);
  const total_pages = useAppSelector(moviesSelectors.getTotalPages);

  const handleLoadMore = async () => {
    if (page < total_pages) {
      await dispatch(moviesThunks.searchMovies({ resetResults: false }));
    }
  };

  if (page >= total_pages || total_pages === null) {
    return null;
  }

  return (
    <Box pb={2} display="flex" justifyContent="center">
      <Box
        position="fixed"
        bottom={16}
        display="flex"
        width="100%"
        justifyContent="center"
      >
        <Button
          variant="contained"
          disabled={isLoading}
          color="primary"
          onClick={handleLoadMore}
        >
          {isLoading ? "Loading..." : "Load More"}
        </Button>
      </Box>
    </Box>
  );
};
