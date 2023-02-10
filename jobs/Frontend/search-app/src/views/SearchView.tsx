import { Navigate } from "react-router-dom";

import { useAppSelector } from "../app/hooks";
import { SearchBar } from "../components/SearchBar/SearchBar";
import { MoviesList } from "../components/MoviesList/MoviesList";
import { selectMoviesState } from "../selectors/movies";

export const SearchView = () => {
  const state = useAppSelector(selectMoviesState);
  return (
    <div>
      <SearchBar />
      {!!state.searchKey && <MoviesList />}
      {state.error && <Navigate to="/error" replace={true} />}
    </div>
  );
};
