import { Box, TextField } from "@mui/material";
import { debounce } from "lodash";
import { ChangeEvent, useMemo } from "react";
import { useAppDispatch, useAppSelector } from "../../../redux/hooks";
import { setQuery } from "../../../redux/movies/movies.slice";
import { moviesSelectors } from "../../../redux/movies/movies.slice.selectors";
import { moviesThunks } from "../../../redux/movies/movies.slice.thunks";

export const SearchInput = () => {
  const dispatch = useAppDispatch();
  const query = useAppSelector(moviesSelectors.getQuery);

  const debouncedSearch = useMemo(
    () =>
      debounce(async (term) => {
        try {
          dispatch(moviesThunks.searchMovies(term));
        } catch (error) {
          console.error(error);
        }
      }, 500),
    [dispatch]
  );

  const handleSearchChange = (
    event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const query = event.target.value;
    dispatch(setQuery(query));
    debouncedSearch(query);
  };

  return (
    <Box sx={{ width: 300, textAlign: "center" }}>
      <TextField
        fullWidth
        size="small"
        value={query}
        onChange={handleSearchChange}
        placeholder="Search for a movie"
      />
    </Box>
  );
};
