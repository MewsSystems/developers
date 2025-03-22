//authentication for API
//the key would usually be handled through .env on the backend, but for the purposes of this fronend task, I simply hardcoded it
const tmdbApiKey = '03b8572954325680265531140190fd2a';

// console.log('hello world');

export interface MoviesData {
  movieArray: Movie[];
  totalPages: number;
}
export const fetchMovies = async (movieKeyword: string, offset: number) => {
  // console.log('hello world 2');
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

  console.log(moviesData);
  return moviesData;
};

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
