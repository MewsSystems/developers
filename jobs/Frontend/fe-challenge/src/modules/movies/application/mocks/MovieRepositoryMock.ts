import { MovieDetail } from '@/modules/movies/domain/MovieDetail';
import { MovieRepository } from '@/modules/movies/domain/MovieRepository';
import { MovieSearchResult } from '@/modules/movies/domain/MovieSearchResult';

export const moviesMock = {
  page: 1,
  results: [],
  totalPages: 0,
  totalResults: 0,
};

export const movieDetailMock = new MovieDetail({
  id: 1,
  title: 'title',
  overview: 'overview',
  voteAverage: 7,
  releaseDate: '2024/04/12',
  backdropImage: 'backdrop-image',
  posterImage: 'poster-image',
  genres: [],
  runtime: 120,
  cast: [],
  directors: [],
  productionCompanies: [],
});

export class MovieRepositoryMock implements MovieRepository {
  async search(): Promise<MovieSearchResult> {
    return moviesMock;
  }

  async getDetail(): Promise<MovieDetail> {
    return movieDetailMock;
  }
}
