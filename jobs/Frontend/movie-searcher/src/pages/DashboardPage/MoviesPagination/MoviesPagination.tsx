import { useDispatch, useSelector } from "react-redux";
import {
  selectMoviesListState,
  setCurrentPage,
} from "../../../store/moviesSearch/movieSearchReducer";
import { Pagination } from "antd";

const MoviesPagination = () => {
  const dispatch = useDispatch();
  const { moviesFound, currentPage } = useSelector(selectMoviesListState);
  const { total_pages: totalPages } = moviesFound || {};

  const pageChangeHandler = (page: number) => {
    console.log("page", page);
    dispatch(setCurrentPage(page));
  };

  return (
    <Pagination total={totalPages as number} current={currentPage} onChange={pageChangeHandler} />
  );
};

export { MoviesPagination };
