import {
  MovieRepositoryMock,
  moviesMock,
} from '@/modules/movies/application/mocks/MovieRepositoryMock';
import { searchMovie } from '@/modules/movies/application/searchMovie';

describe('searchMovie', () => {
  test('should get all movies', async () => {
    const repository = new MovieRepositoryMock();

    const movies = await searchMovie(repository, 'Batman', 1);

    expect(movies).toEqual(moviesMock);
  });
});
