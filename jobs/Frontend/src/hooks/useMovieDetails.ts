import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../redux/hooks";
import { moviesSelectors } from "../redux/movies/movies.slice.selectors";
import { moviesThunks } from "../redux/movies/movies.slice.thunks";
import { useParams } from "react-router-dom";

export const useMovieDetails = () => {
  const params = useParams<{ id: string }>();
  const dispatch = useAppDispatch();
  const selectedMovie = useAppSelector(moviesSelectors.getSelectedMovie);

  useEffect(() => {
    if (!selectedMovie && params.id) {
      dispatch(moviesThunks.fetchMovieDetails(Number(params.id)));
    }
  }, [dispatch, params.id, selectedMovie]);
};
