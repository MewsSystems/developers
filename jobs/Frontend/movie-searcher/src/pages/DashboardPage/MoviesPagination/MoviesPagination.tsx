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
  const { totalPages, currentPage } = useSelector(selectMoviesListState);
  const { inputValue } = useSelector(selectMoviesListState);

  const pageChangeHandler = (page: number) => {
    dispatch(setCurrentPage(page));
  };

  useEffect(() => {
    if (currentPage !== 1) {
      dispatch(getMoviesList({ value: inputValue || "", page: currentPage }));
    }
  }, [dispatch, currentPage, getMoviesList]);

  return (
    <Pagination total={totalPages as number} current={currentPage} onChange={pageChangeHandler} />
  );
};

export { MoviesPagination };
