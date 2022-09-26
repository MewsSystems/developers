import { NextPage } from "next";
import { ChangeEventHandler, useCallback } from "react";

import { useAppDispatch, useAppSelector } from "../common/hooks";
import { MovieList } from "./components/MovieList";
import { ListSearchInput } from "./components/MovieList/parts/ListSearchInput";
import {
  changeSearchQuery,
  resetPagination,
  selectQuery,
} from "./redux/movieListSlice";

export const MovieListPage: NextPage = () => {
  const query = useAppSelector<string>(selectQuery);
  const dispatch = useAppDispatch();

  const searchInputChange: ChangeEventHandler<HTMLInputElement> = useCallback(
    (event) => {
      dispatch(changeSearchQuery(event.target.value));
      dispatch(resetPagination());
    },
    [dispatch]
  );
  return (
    <>
      <h1>The Movie List</h1>
      <ListSearchInput onChange={searchInputChange} value={query} />
      <MovieList />
    </>
  );
};
