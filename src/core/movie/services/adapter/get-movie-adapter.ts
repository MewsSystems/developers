import { Movie } from '../../types/movie';

export const getMovieDetailsAdapter = (movie: any): Movie => {
  return {
    id: movie.id ?? null,
    title: movie.title ?? null,
    overview: movie.overview ?? null,
    posterPath: movie.poster_path ?? null,
    releaseDate: movie.release_date ?? null,
    voteAverage: movie.vote_average ?? null,
    voteCount: movie.vote_count ?? null,
    popularity: movie.popularity ?? null,
    backdropPath: movie.backdrop_path ?? null,
    language: movie.original_language ?? null,
    video: movie.video ?? false,
    runtime: movie.runtime ?? null,
  };
};
