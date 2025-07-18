import React, { useCallback } from "react";
import debounce from "debounce";
import { searchMovies, setSearchQuery } from "../../state/movies/moviesSlice";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../../state/store";
import { StyledInput } from "./Search.styles";

const Search = () => {
  const dispatch = useDispatch<AppDispatch>();
  const searchQuery = useSelector(
    (state: RootState) => state.movies.searchQuery
  );

  const onSearchRequest = useCallback(
    (searchQuery: string) => {
      dispatch(searchMovies({ searchQuery, page: 1 }));
    },
    [dispatch]
  );

  // ESlint doesn't like useCallback with debounce
  // eslint-disable-next-line react-hooks/exhaustive-deps
  const debouncedonSearchRequest = useCallback(debounce(onSearchRequest, 500), [
    onSearchRequest,
  ]);

  const onSearchInputChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      dispatch(setSearchQuery(e.target.value));
      debouncedonSearchRequest(e.target.value);
    },
    [dispatch, debouncedonSearchRequest]
  );

  return (
    <StyledInput
      type="text"
      placeholder="Search for movies..."
      value={searchQuery}
      onChange={onSearchInputChange}
    />
  );
};

export default Search;
