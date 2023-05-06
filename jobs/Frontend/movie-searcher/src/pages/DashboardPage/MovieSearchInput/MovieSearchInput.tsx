import debounce from "lodash/debounce";
import { ChangeEvent, useCallback, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AnyAction, ThunkDispatch } from "@reduxjs/toolkit";
import {
  getMoviesList,
  selectMoviesListState,
  setCurrentPage,
  setInputValue,
} from "../../../store/moviesSearch/movieSearchReducer";
import { SEARCH_DEBOUNCE_DELAY } from "../../../constants";
import { MovieSearchInputView } from "./MovieSearchInputView";

const MovieSearchInput = () => {
  const dispatch: ThunkDispatch<unknown, unknown, AnyAction> = useDispatch();
  const { isLoading } = useSelector(selectMoviesListState);
  const { inputValue } = useSelector(selectMoviesListState);

  const inputValueChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
    dispatch(setInputValue(e.target.value));
  };

  const onSearchDebounced = useCallback(
    debounce((value) => {
      if (value?.length === 0 || value?.length >= 2) {
        dispatch(setCurrentPage(1));
        dispatch(getMoviesList({ value }));
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
