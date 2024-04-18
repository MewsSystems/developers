import { Dispatch, SetStateAction } from 'react';

export const getMovies = async (
  searchValue: string,
  setMoviesData: Dispatch<SetStateAction<never[]>>,
) => {
  const options = {
    method: 'GET',
    headers: {
      accept: 'application/json',
    },
  };
  const response = await fetch(
    `https://api.themoviedb.org/3/search/movie?query=${searchValue}&api_key=03b8572954325680265531140190fd2a`,
    options,
  );
  const moviesData = await response.json();

  console.log('moviesData', moviesData);
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
