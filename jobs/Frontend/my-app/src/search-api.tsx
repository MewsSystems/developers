//authentication for API
const tmdbApiKey = import.meta.env.VITE_TMDB_API_KEY;

export interface Movie {
  id: number;
  title: string;
  release_date: string;
  vote_average: string;
  poster_path: string;
}

export interface MoviesData {
  movieArray: Movie[];
  totalPages: number;
}

// MOVIE SEARCH
export const fetchMovies = async (movieKeyword: string, offset: number) => {
  const moviesPerPage = 20;
  const fetchPage = Math.floor(offset / moviesPerPage) + 1;
  const indexInPage = offset % moviesPerPage;

  const response = await fetch(
    `https://api.themoviedb.org/3/search/movie?api_key=${tmdbApiKey}&query=${movieKeyword}&page=${fetchPage}`
  );
  const data = await response.json();

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const moviesData: MoviesData = {
    movieArray: data.results.slice(indexInPage, indexInPage + 10),
    totalPages: Math.ceil(data.total_results / 10),
  };

  return moviesData;
};

// POPULAR MOVIES
export const fetchPopularMovies = async (offset: number) => {
  const moviesPerPage = 20;
  const fetchPage = Math.floor(offset / moviesPerPage) + 1;
  const indexInPage = offset % moviesPerPage;

  const response = await fetch(
    `https://api.themoviedb.org/3/movie/popular?api_key=${tmdbApiKey}&page=${fetchPage}`
  );
  const data = await response.json();

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const popularMoviesData: MoviesData = {
    movieArray: data.results.slice(indexInPage, indexInPage + 10),
    totalPages: Math.ceil(data.total_results / 10),
  };

  return popularMoviesData;
};

//MOVIE DETAILS
export const fetchMovieDetails = async (movieId: number) => {
  const response = await fetch(
    `https://api.themoviedb.org/3/movie/${movieId}?api_key=${tmdbApiKey}`
  );

  const details = await response.json();

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  return details;
};
