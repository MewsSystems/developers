import { FC, useCallback } from "react";

import { useAppDispatch, useAppSelector } from "~/features/common/hooks";
import { useGetSearchMoviesQuery } from "../../hooks";
import {
  changePagination,
  selectPagination,
  selectQuery,
} from "../../redux/movieListSlice";
import type { MovieList as MovieListType } from "../../types";
import { ListPagination } from "./parts/ListPagination";
import { MovieListItem } from "./parts/MovieListItem";
import { List } from "./styled";

type PageChangeHandlerFn = (selectItem: { selected: number }) => void;

export const MovieList: FC = () => {
  const query = useAppSelector(selectQuery);
  const currentPage = useAppSelector(selectPagination);
  const dispatch = useAppDispatch();

  const { data, isLoading } = useGetSearchMoviesQuery({
    query: query,
    page: currentPage,
  });
  const movies = (data?.results || []) as MovieListType;

  const pageChangeHandler: PageChangeHandlerFn = useCallback(
    ({ selected }) => {
      dispatch(changePagination(selected + 1));
      window.scrollTo({ top: 0, left: 0, behavior: "smooth" });
    },
    [dispatch]
  );

  if (!query.length) {
    return null;
  }

  if (isLoading) {
    // TODO: create loading component
    return <p>Results are loading</p>;
  }

  if (!data) {
    // TODO: create error component
    return <p>Error loading movie list</p>;
  }

  return (
    <>
      {!!query.length && (
        <List>
          {!!movies.length &&
            movies.map((movieItem) => {
              return (
                <MovieListItem
                  movie={movieItem}
                  key={movieItem.id.toString()}
                />
              );
            })}
        </List>
      )}
      {/* TODO: create not found component */}
      {!movies.length && <h3>No results found for you search.</h3>}
      <ListPagination
        pageCount={data.total_pages}
        renderOnZeroPageCount={() => null}
        onPageChange={pageChangeHandler}
      />
    </>
  );
};
