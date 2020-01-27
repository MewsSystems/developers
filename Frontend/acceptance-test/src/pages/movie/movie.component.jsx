import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";

//Utils
import { fetchMovie } from "../../redux/movie/movie.utils";

//Abort Controller
import { abortController } from "../../utils/abort-controller";

//Component
import { Spinner } from "../../components/spinner/spinner.component";
import MovieDetails from "../../components/movie-details/movie-details.component";

//Styles
import { Poster, MovieContainer } from "./movie.styles";

const Movie = ({ match }) => {
  const {
    params: { movieId }
  } = match;

  const { isLoading, errors, movie } = useSelector(state => state.movie);
  const dispatch = useDispatch();
  const getMovie = () => dispatch(fetchMovie(movieId));

  useEffect(() => {
    getMovie();
    return () => {
      abortController.abort();
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <>
      {isLoading ? (
        <Spinner />
      ) : (
        <MovieContainer>
          <Poster
            imageBg={`https://image.tmdb.org/t/p/w185/${movie.poster_path}`}
            alt={`${movie.title}`}
          />
          <div className="movie-details">
            <MovieDetails errors={errors} movieDetails={movie} />
          </div>
        </MovieContainer>
      )}
    </>
  );
};

export default Movie;
