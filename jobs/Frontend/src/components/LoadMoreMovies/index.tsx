import { Button } from "@mui/material";
import { FC } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import { moviesSelectors } from "../../redux/movies/movies.slice.selectors";
import { moviesThunks } from "../../redux/movies/movies.slice.thunks";

export const LoadMoreMovies: FC = () => {
  const dispatch = useAppDispatch();
  const page = useAppSelector(moviesSelectors.getSearchPage);
  const total_pages = useAppSelector(moviesSelectors.getTotalPages);

  const handleLoadMore = async () => {
    if (page < total_pages) {
      await dispatch(moviesThunks.searchMovies());
    }
  };

  if (page >= total_pages || total_pages === null) {
    return null;
  }

  return (
    <Button variant="contained" color="primary" onClick={handleLoadMore}>
      Load More
    </Button>
  );
};
