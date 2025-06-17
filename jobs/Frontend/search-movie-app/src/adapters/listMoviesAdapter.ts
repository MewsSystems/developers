import type { ListMoviesResponse } from '../api/types';
import type { ListMovie } from '../types/movieTypes';

const listMoviesAdapter = (listMoviesData: ListMoviesResponse) => {
  const listMovies: ListMovie[] = listMoviesData.results.map(movie => {
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
  return {
    page: listMoviesData.page,
    totalPages: listMoviesData.total_pages,
    totalMovieList: listMoviesData.total_results,
    listMovies: listMovies,
  };
};

export { listMoviesAdapter };
