import { MovieResponse } from './types/movie-response';

export const isValidMoviesResponse = (data: unknown): data is MovieResponse => {
  return (
    typeof data === 'object' &&
    data !== null &&
    'results' in data &&
    Array.isArray((data as any).results) &&
    'page' in data &&
    'total_pages' in data &&
    'total_results' in data
  );
};

export const isValidMovieResponse = (data: unknown): data is { id: number } => {
  return (
    typeof data === 'object' &&
    data !== null &&
    'id' in data &&
    typeof (data as any).id === 'number'
  );
};
