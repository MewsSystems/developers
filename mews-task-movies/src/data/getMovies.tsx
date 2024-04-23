import { Dispatch, SetStateAction } from 'react';
import { Movie } from './interfaces';

export const getMoviesWithActualParametres = async (
  searchValue: string,
  setMoviesData: Dispatch<SetStateAction<Movie[]>>,
  pageNumber: number,
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

const getMovies = async (searchValue: string, pageNumber: number) => {
  const response = await fetch(
    `${process.env.REACT_APP_API_BASE_PATH}/search/movie?query=${searchValue}&page=${pageNumber}&api_key=${process.env.REACT_APP_API_KEY}`,
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
  setSelectedMovieData: React.Dispatch<Movie>,
) => {
  const options = {
    method: 'GET',
    headers: {
      accept: 'application/json',
    },
  };
  const response = await fetch(
    `${process.env.REACT_APP_API_BASE_PATH}/movie/${movieId}?api_key=${process.env.REACT_APP_API_KEY}`,
    options,
  );
  const movieData = await response.json();
  setSelectedMovieData(movieData);
};
