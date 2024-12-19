import {
  MovieDetails,
  MovieDetailsTmdbApi,
  MovieResponse as MovieResponse,
  MovieTmdbApi,
  MovieTmdbApiResponse,
} from '../ models/movieModel';

const fetchPopularMovies = async (page: number): Promise<MovieTmdbApiResponse> => {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/movie/popular?page=${page}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${import.meta.env.VITE_API_KEY}`,
    },
  });

  return response.json();
};

const fetchSearchMovies = async (search: string, page: number): Promise<MovieTmdbApiResponse> => {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/search/movie?query=${search}&page=${page}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${import.meta.env.VITE_API_KEY}`,
    },
  });
  return response.json();
};

export const getMovies = async (search: string, page: number): Promise<MovieResponse> => {
  try {
    let moviesResponse: MovieTmdbApiResponse;

    if (search) {
      moviesResponse = await fetchSearchMovies(search, page);
    } else {
      moviesResponse = await fetchPopularMovies(page);
    }

    const movies = moviesResponse.results.map((movie: MovieTmdbApi) => {
      return {
        id: movie.id,
        poster: movie.poster_path,
        title: movie.title,
      };
    });

    return {
      page: moviesResponse.page,
      totalPages: moviesResponse.total_pages,
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
  const response = await fetch(`${import.meta.env.VITE_API_URL}/movie/${movieId}?language=en-US`, {
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
