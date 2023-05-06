import { Divider } from "antd";
import { useSelector } from "react-redux";
import { selectMoviesListState } from "../../store/moviesSearch/movieSearchReducer";
import { LoadMore } from "./LoadMore";
import { MovieSearchInput } from "./MovieSearchInput";
import { MoviesList } from "./MoviesList";
import { MoviesPagination } from "./MoviesPagination";

const DashboardPage = () => {
  const { totalPages, currentPage } = useSelector(selectMoviesListState);
  console.log("totalPages", totalPages);

  return (
    <>
      <MovieSearchInput />
      <Divider />
      <MoviesList />
      {Number(totalPages) > 1 && (
        <>
          {currentPage < Number(totalPages) && <LoadMore />}
          <Divider />
          <MoviesPagination />
        </>
      )}
    </>
  );
};

export { DashboardPage };
