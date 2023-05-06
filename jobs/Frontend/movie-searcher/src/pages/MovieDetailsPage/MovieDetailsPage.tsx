import { useEffect, useMemo } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { AnyAction } from "redux";
import { ThunkDispatch } from "redux-thunk";
import { TMDB_IMAGES_URL } from "../../constants";
import {
  getMovieDetails,
  selectMovieDetailsState,
} from "../../store/movieDetails/movieDetailsReducer";
import MovieDetailsPageView from "./MovieDetailsPageView";
import { normalizeMovieDetails } from "./normalize.util";

const MovieDetailsPage = () => {
  const { movieId } = useParams();
  const dispatch: ThunkDispatch<unknown, unknown, AnyAction> = useDispatch();
  const { movie } = useSelector(selectMovieDetailsState);

  const normalizedMovieDetails = useMemo(() => normalizeMovieDetails(movie), [movie]);
  const imgUrlPath = `${TMDB_IMAGES_URL}/${normalizedMovieDetails?.imgUrl}`;

  useEffect(() => {
    dispatch(getMovieDetails(movieId || ""));
  }, []);

  return <MovieDetailsPageView {...normalizedMovieDetails} imgUrl={imgUrlPath} />;
};

export { MovieDetailsPage };
