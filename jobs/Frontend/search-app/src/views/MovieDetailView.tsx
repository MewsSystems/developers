import { useEffect } from "react";
import { Navigate, useParams } from "react-router-dom";

import { useAppDispatch, useAppSelector } from "../app/hooks";
import { Spinner } from "../components/Spinner/Spinner";

import { MovieDetail } from "../components/MovieDetail/MovieDetail";
import { selectMoviesState } from "../selectors/movies";
import { fetchMovieDetail } from "../actions/fetchMovies";
import { Container } from "./MovieDetailView.styled";

const MovieDetailView = () => {
  const { movieId } = useParams();
  const dispatch = useAppDispatch();
  const state = useAppSelector(selectMoviesState);

  useEffect(() => {
    if (movieId) {
      dispatch(fetchMovieDetail(movieId));
    }
  }, [movieId, dispatch]);
  return (
    <Container>
      {state.isBusy && <Spinner />}
      {state.movieDetail && <MovieDetail />}
      {state.error && <Navigate to="/error" replace={true} />}
    </Container>
  );
};

export default MovieDetailView;
