import { MovieApi } from '../ models/movieModel';

export const getMovies = async (search: string) => {
  try {
    const response = await fetch(`https://api.themoviedb.org/3/search/movie?query=${search}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${import.meta.env.VITE_API_KEY}`,
      },
    });

    const responseJson = await response.json();

    const movies = responseJson.results.map((movie: MovieApi) => {
      return {
        id: movie.id,
        poster: movie.poster_path,
        release_date: movie.release_date,
        title: movie.title,
      };
    });

    return movies;
  } catch (error) {
    console.log(error);
  }

  return [];
};
