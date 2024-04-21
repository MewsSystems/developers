import { Dispatch, SetStateAction } from 'react';

export const getMoviesWithActualParametres = async (
  searchValue: string,
  setMoviesData: Dispatch<SetStateAction<never[]>>,
  pageNumber: Number,
  setTotalPagesNumber: React.Dispatch<SetStateAction<number>>,
) => {
  if (searchValue) {
    const moviesData = await getMovies(searchValue, pageNumber);
    setTotalPagesNumber(moviesData.total_pages);
    setMoviesData(moviesData.results);
  } else {
    setTotalPagesNumber(1);
    setMoviesData([]);
  }
};

const getMovies = async (searchValue: string, pageNumber: Number) => {
  const response = await fetch(
    `https://api.themoviedb.org/3/search/movie?query=${searchValue}&page=${pageNumber}&api_key=${process.env.REACT_APP_API_KEY}`,
    {
      method: 'GET',
      headers: {
        accept: 'application/json',
      },
    },
  );

  return await response.json();
};

export const getMovieById = async (
  movieId: number,
  setSelectedMovieData: React.Dispatch<Object>,
) => {
  const options = {
    method: 'GET',
    headers: {
      accept: 'application/json',
    },
  };
  const response = await fetch(
    `https://api.themoviedb.org/3/movie/${movieId}?api_key=${process.env.REACT_APP_API_KEY}`,
    options,
  );
  const movieData = await response.json();
  setSelectedMovieData(movieData);
};
