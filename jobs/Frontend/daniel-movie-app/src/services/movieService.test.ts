import { fetchMovies } from './movieService';

test('fetchMovies return maximum of 20 movies', async () => {
  const movies = await fetchMovies('movie');
  expect(movies.results.length).toBe(20);
});
