import { useDispatch, useSelector } from "react-redux";
import { MovieDto } from "../../dtos/movieDto";
import "./MovieListItem.css";
import React from "react";
import { DispatchAction } from "../../models/dispatchAction";
import {
  SET_SELECTED_MOVIE,
  SET_SELECTED_MOVIE_GENRES
} from "../../models/actions";
import { InitialState } from "../../models/initialState";

const MovieListItem: React.FC<{
  movie: MovieDto;
}> = ({ movie }) => {
  const dispatch = useDispatch();
  const genres = useSelector((state: InitialState) => state.genres);

  function setMovieGenres(movie: MovieDto) {
    const newMovieGenres =
      genres
        ?.filter((x) => movie.genre_ids?.includes(x.id))
        ?.map((x) => x.name) || [];
    dispatch<DispatchAction>({
      type: SET_SELECTED_MOVIE_GENRES,
      value: newMovieGenres
    });
  }

  function setSelectedMovie(movie: MovieDto) {
    setMovieGenres(movie);
    dispatch<DispatchAction>({
      type: SET_SELECTED_MOVIE,
      value: movie
    });
  }

  function defineVoteAverageColor(vote: number) {
    return vote >= 7 ? "green" : vote >= 5 ? "orange" : "red";
  }

  return (
    <div
      onClick={() => setSelectedMovie(movie)}
      className="movieListItem d-flex flex-column align-items-center">
      <div className="voteAverage d-flex justify-content-center align-items-center">
        <p style={{ color: defineVoteAverageColor(movie.vote_average) }}>
          {movie.vote_average.toPrecision(2)}
        </p>
      </div>
      <img
        src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
        width="100%"
        alt={movie.title}
      />
      <h6 className="m-3">{movie.title}</h6>
    </div>
  );
};

export default MovieListItem;
