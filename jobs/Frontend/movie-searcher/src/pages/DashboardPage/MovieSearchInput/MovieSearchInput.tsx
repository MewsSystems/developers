import debounce from "lodash/debounce";
import { useCallback, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AnyAction, ThunkDispatch } from "@reduxjs/toolkit";
import {
  getMoviesList,
  selectMoviesListState,
} from "../../../store/moviesSearch/movieSearchReducer";
import { SEARCH_DEBOUNCE_DELAY } from "../../../constants";
import { MovieSearchInputView } from "./MovieSearchInputView";
import { useInputChange } from "../../../helpers/hooks/useInputChange.hook";

const MovieSearchInput = () => {
  const dispatch: ThunkDispatch<unknown, unknown, AnyAction> = useDispatch();
  const { isLoading } = useSelector(selectMoviesListState);
  const { inputValue, inputValueChangeHandler } = useInputChange();

  const onSearchDebounced = useCallback(
    debounce((value) => {
      if (value?.length === 0 || value?.length >= 3) {
        dispatch(getMoviesList(value));
      }
    }, SEARCH_DEBOUNCE_DELAY),
    [dispatch, getMoviesList]
  );

  useEffect(() => {
    onSearchDebounced(inputValue);
  }, [inputValue, onSearchDebounced]);

  return (
    <MovieSearchInputView
      searchInput={inputValue}
      searchChangeHandler={inputValueChangeHandler}
      isLoading={isLoading}
    />
  );
};

export { MovieSearchInput };
