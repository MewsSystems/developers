import type { Movie, TmdbMovie } from "../types";
import { TMDB_IMAGE_BASE, TMDB_POSTER_SIZE } from "../constants";

export const TMDB_MOVIE_INTERSTELLAR: TmdbMovie = {
  id: 123,
  title: "Interstellar",
  poster_path: "/interstellar.jpg",
  release_date: "2014-11-07",
  overview: "A team of explorers travel through a wormhole in space.",
  popularity: 98.4,
  vote_average: 8.6
}

export const MOVIE_INTERSTELLAR: Movie = {
  id: 123,
  title: "Interstellar",
  posterPath: `${TMDB_IMAGE_BASE}/${TMDB_POSTER_SIZE}/interstellar.jpg`,
  releaseDate: "2014-11-07",
  formattedReleaseDate: "7 November 2014",
  overview: "A team of explorers travel through a wormhole in space.",
  popularity: 98.4,
  voteAverage: 8.6
};

export const MOVIE_INCEPTION: Movie = {
  id: 42,
  title: "Inception",
  posterPath: `${TMDB_IMAGE_BASE}/${TMDB_POSTER_SIZE}/inception.jpg`,
  releaseDate: "2010-07-16",
  formattedReleaseDate: "16 July 2010",
  overview: "A thief steals secrets through dream-sharing technology.",
  popularity: 90.1,
  voteAverage: 8.3
};