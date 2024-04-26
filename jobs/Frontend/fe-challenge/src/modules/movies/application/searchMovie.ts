import { MovieRepository } from '@/modules/movies/domain/MovieRepository';
import { MovieSearchResult } from '@/modules/movies/domain/MovieSearchResult';

export const searchMovie = (
  movieRepository: MovieRepository,
  query: string,
  page: number,
): Promise<MovieSearchResult> => {
  return movieRepository.search(query, page);
};
