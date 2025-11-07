import { MovieDetail } from '@/modules/movies/domain/MovieDetail';
import { MovieSearchResult } from '@/modules/movies/domain/MovieSearchResult';

export interface MovieRepository {
  search: (query: string, page: number) => Promise<MovieSearchResult>;
  getDetail: (id: number) => Promise<MovieDetail>;
}
