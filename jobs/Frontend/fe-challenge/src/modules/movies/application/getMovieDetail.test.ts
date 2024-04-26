import { getMovieDetail } from '@/modules/movies/application/getMovieDetail';
import {
  MovieRepositoryMock,
  movieDetailMock,
} from '@/modules/movies/application/mocks/MovieRepositoryMock';

describe('getMovieDetail', () => {
  test('should get a movie detail', async () => {
    const repository = new MovieRepositoryMock();

    const movieDetail = await getMovieDetail(repository, 1);

    expect(movieDetail).toEqual(movieDetailMock);
  });
});
