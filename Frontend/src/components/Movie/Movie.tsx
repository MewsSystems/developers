import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';

import config from '../../config';
import { fetchMovie, resetMovie } from '../../actions/movie';
import { RootReducer } from '../../reducers';

import {
  LoadingWrapper,
  BackButton,
  MoviePageWrapper,
  MoviePoster,
  MovieTitle,
  MovieOriginalTitle,
  MovieTagline,
  MovieGenres,
  MovieGenre,
  MovieVote,
  MovieVoteNum,
} from './Styled';
import { MoviePosterImage } from '../Search/Styled';

type MovieOwnProps = {
  id: string;
};

const Movie = (props: MovieOwnProps) => {
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(fetchMovie(props.id));

    return () => {
      dispatch(resetMovie());
    };
  }, [dispatch, props.id]);

  const movie = useSelector((state: RootReducer) => state.movie);

  if (!movie.id) {
    return (
      <LoadingWrapper>
        <span>Loading...</span>
      </LoadingWrapper>
    );
  }

  return (
    <div>
      <BackButton to="/">Back</BackButton>
      <MoviePageWrapper>
        <MoviePoster>
          {movie.poster_path ? (
            <MoviePosterImage
              src={`${config.IMAGE_BASE_URL}${movie.poster_path}`}
              alt={movie.title}
              title={movie.title}
            />
          ) : (
            <div>{movie.title}</div>
          )}
        </MoviePoster>
        <div>
          <MovieTitle>
            {movie.title} {movie.release_date && <span>{` (${new Date(movie.release_date).getFullYear()})`}</span>}
          </MovieTitle>
          {movie.title !== movie.original_title && <MovieOriginalTitle>{movie.original_title}</MovieOriginalTitle>}
          {
            <MovieVote>
              <MovieVoteNum vote={movie.vote_average}>{movie.vote_average}</MovieVoteNum>/10 ({movie.vote_count})
            </MovieVote>
          }
          {movie.tagline && (
            <MovieTagline>
              <i>{movie.tagline}</i>
            </MovieTagline>
          )}

          {movie.genres.length > 0 && (
            <MovieGenres>
              {movie.genres.map((it) => (
                <MovieGenre key={it.id}>{it.name}</MovieGenre>
              ))}
            </MovieGenres>
          )}

          <div>{movie.overview}</div>
        </div>
      </MoviePageWrapper>
    </div>
  );
};

export default Movie;
