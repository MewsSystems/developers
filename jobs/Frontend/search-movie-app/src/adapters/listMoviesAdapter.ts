import type { ListMovie, Movie } from '../types/movieTypes';

export const listMoviesAdapter = (listMoviesData: Movie[]): ListMovie[] => {
  const listMovies: ListMovie[] = listMoviesData.map(movie => {
    return {
      isAdult: movie.adult,
      id: movie.id,
      imageURL: movie.poster_path,
      releaseDate: movie.release_date,
      title: movie.title,
      voteAverage: movie.vote_average,
      voteTotalCount: movie.vote_count,
    };
  });
  return listMovies;
};
