import { Divider } from "antd";
import { useSelector } from "react-redux";
import { selectMoviesListState } from "../../store/moviesSearch/movieSearchReducer";
import { MovieSearchInput } from "./MovieSearchInput";
import { MoviesList } from "./MoviesList";
import { MoviesPagination } from "./MoviesPagination";

const DashboardPage = () => {
  const { totalPages } = useSelector(selectMoviesListState);

  return (
    <>
      <MovieSearchInput />
      <Divider />
      <MoviesList />
      {Number(totalPages) > 1 && (
        <>
          <Divider />
          <MoviesPagination />
        </>
      )}
    </>
  );
};

export { DashboardPage };
