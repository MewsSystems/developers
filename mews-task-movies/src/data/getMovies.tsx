import { Dispatch, SetStateAction } from 'react';

export const getMovies = async (
  searchValue: string,
  setMoviesData: Dispatch<SetStateAction<never[]>>,
  pageNumber: Number,
  setTotalPagesNumber: React.Dispatch<SetStateAction<number>>,
) => {
  console.log('process.env.API_KEY', process.env, process.env.API_KEY);
  const response = await fetch(
    `https://api.themoviedb.org/3/search/movie?query=${searchValue}&page=${pageNumber}&api_key=${process.env.REACT_APP_API_KEY}`,
    {
      method: 'GET',
      headers: {
        accept: 'application/json',
      },
    },
  );
  const moviesData = await response.json();

  console.log('moviesData', moviesData);
  setTotalPagesNumber(moviesData.total_pages);
  setMoviesData(moviesData.results);
  console.log('moviesData.result', moviesData.results);
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
    `https://api.themoviedb.org/3/movie/${movieId}?api_key=03b8572954325680265531140190fd2a`,
    options,
  );
  const movieData = await response.json();
  console.log('movieData', movieData);
  setSelectedMovieData(movieData);
};
