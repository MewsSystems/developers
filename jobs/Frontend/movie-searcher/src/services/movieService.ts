import { Movie } from '../ models/movieModel';
import responseMovies from '../mocks/avengers.json';

export const getMovies = (): Movie[] => {
  const movies = responseMovies.results.map((movie) => {
    return {
      id: movie.id,
      poster: movie.poster_path,
      release_date: movie.release_date,
      title: movie.title,
    };
  });
  return movies;
};
