import { useDispatch, useSelector } from "react-redux";
import { AnyAction } from "redux";
import { ThunkDispatch } from "redux-thunk";
import { useEffect } from "react";
import { Pagination } from "antd";
import {
  getMoviesList,
  selectMoviesListState,
  setCurrentPage,
} from "../../../store/moviesSearch/movieSearchReducer";

const MoviesPagination = () => {
  const dispatch: ThunkDispatch<unknown, unknown, AnyAction> = useDispatch();
  const { totalResults, currentPage, inputValue, visiblePages } =
    useSelector(selectMoviesListState);

  const pageChangeHandler = (page: number) => {
    dispatch(setCurrentPage(page));
  };

  useEffect(() => {
    if (!visiblePages.includes(currentPage)) {
      dispatch(getMoviesList({ value: inputValue || "", page: currentPage }));
    }
  }, [dispatch, currentPage, getMoviesList]);

  return (
    <Pagination
      total={totalResults as number}
      current={currentPage}
      onChange={pageChangeHandler}
      defaultPageSize={20}
    />
  );
};

export { MoviesPagination };
