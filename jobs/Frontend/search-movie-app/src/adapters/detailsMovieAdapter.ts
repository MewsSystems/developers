import type { DetailsMovie, MovieDetails } from '../types';

export const detailsMovieAdapter = (detailsMovie: MovieDetails): DetailsMovie => {
  return {
    isAdult: detailsMovie.adult,
    id: detailsMovie.id,
    imageURL: detailsMovie.poster_path,
    releaseDate: detailsMovie.release_date,
    title: detailsMovie.title,
    voteAverage: detailsMovie.vote_average,
    voteTotalCount: detailsMovie.vote_count,
    genres: detailsMovie.genres,
    tagline: detailsMovie.tagline,
    overview: detailsMovie.overview,
    mobileImageURL: detailsMovie.backdrop_path,
  };
};
