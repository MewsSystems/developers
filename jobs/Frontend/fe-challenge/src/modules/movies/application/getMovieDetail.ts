import { MovieDetail } from '@/modules/movies/domain/MovieDetail';
import { MovieRepository } from '@/modules/movies/domain/MovieRepository';

export const getMovieDetail = (
  movieRepository: MovieRepository,
  id: number,
): Promise<MovieDetail> => {
  return movieRepository.getDetail(id);
};
