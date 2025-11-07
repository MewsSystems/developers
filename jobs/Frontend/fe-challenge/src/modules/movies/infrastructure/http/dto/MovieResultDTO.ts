import { MovieDTO } from '@/modules/movies/infrastructure/http/dto/MovieDTO';

export interface MovieResultDTO {
  page: number;
  results: Array<MovieDTO>;
  total_pages: number;
  total_results: number;
}
