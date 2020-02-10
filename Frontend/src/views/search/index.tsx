import React from "react";
import { useDispatch, useSelector } from "react-redux";

import { Pagination } from "../../components/pagination";
import { Spinner } from "../../components/spinner";
import { AppState } from "../../reducers";
import { movieSearch } from "../../actions/movie-search";
import { SearchBox, MovieList } from "./components";
import { ErrorMessage } from "../../components";

export const SearchView: React.FC = () => {
  const dispatch = useDispatch()
  const isBusy = useSelector(({ movieSearch }: AppState) => movieSearch.isFetching );
  const { currentPage, totalPages, query, error, movies } = useSelector(({ movieSearch }: AppState) => ({
    totalPages: movieSearch.total_pages,
    currentPage: movieSearch.page,
    query: movieSearch.query,
    error: movieSearch.error,
    movies: movieSearch.results
  }))

  const onPageChange = (page: number) => {
    dispatch(movieSearch(query, page))
  }

  const onSearch = (query:string) => {
    dispatch(movieSearch(query, 1))
  }

  if (error) {
    return (
      <ErrorMessage>
        Something went wrong... :(
      </ErrorMessage>
    )
  }

  return (
    <>
      <SearchBox isBusy={!!isBusy} onSearch={onSearch} />
      <Pagination buttonAmount={5} totalPages={totalPages} current={currentPage} onPageChange={onPageChange} />
      {isBusy && <Spinner />}
      {totalPages !== 0 && <MovieList movies={movies} />}
      <Pagination buttonAmount={5} totalPages={totalPages} current={currentPage} onPageChange={onPageChange} />
    </>
  )
}
