import { Movie } from '@movie/types/movie';

type MoviePoster = Pick<Movie, 'posterPath'>;

export const getMovieImageUrl = (baseURL: string, movie: MoviePoster): string =>
  `${baseURL}${movie.posterPath}`;
