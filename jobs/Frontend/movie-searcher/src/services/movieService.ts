import { MovieDetails, MovieDetailsTmdbApi, MovieResponse as MovieResponse, MovieTmdbApi } from '../ models/movieModel';

export const getMovies = async (search: string, page: number): Promise<MovieResponse> => {
  try {
    const response = await fetch(`https://api.themoviedb.org/3/search/movie?query=${search}&page=${page}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${import.meta.env.VITE_API_KEY}`,
      },
    });

    const responseJson = await response.json();

    const movies = responseJson.results.map((movie: MovieTmdbApi) => {
      return {
        id: movie.id,
        poster: movie.poster_path,
        title: movie.title,
      };
    });

    return {
      page: responseJson.page,
      totalPages: responseJson.total_pages,
      movies,
    };
  } catch (error) {
    console.log(error);
  }

  return {
    page: 0,
    totalPages: 0,
    movies: [],
  };
};

export const getMovie = async (movieId: string): Promise<MovieDetails> => {
  const response = await fetch(`https://api.themoviedb.org/3/movie/${movieId}?language=en-US`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${import.meta.env.VITE_API_KEY}`,
    },
  });

  const responseJson: MovieDetailsTmdbApi = await response.json();
  const genres = responseJson.genres.map((genre) => genre.name);

  return {
    genres,
    id: responseJson.id,
    overview: responseJson.overview,
    poster: responseJson.poster_path,
    releaseDate: responseJson.release_date,
    title: responseJson.title,
  };
};
