import { Divider } from "antd";
import { useSelector } from "react-redux";
import { selectMoviesListState } from "../../store/moviesSearch/movieSearchReducer";
import { MovieSearchInput } from "./MovieSearchInput";
import { MoviesList } from "./MoviesList";
import { MoviesPagination } from "./MoviesPagination";

const DashboardPage = () => {
  const { moviesFound } = useSelector(selectMoviesListState);

  return (
    <>
      <MovieSearchInput />
      <Divider />
      <MoviesList />
      <Divider />
      {Number(moviesFound?.total_pages) > 1 && <MoviesPagination />}
    </>
  );
};

export { DashboardPage };
