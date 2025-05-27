import { getMoviesAdapter } from "../adapter/get-movies-adapter";
import { movieApi } from "./movie-api";
import { isValidMoviesResponse } from "./validation";
import { MoviesDataList } from "./types/movie-data-list";

export const getMovies = async (page: number = 1): Promise<MoviesDataList> => {
    try {
      const response = await movieApi.get({ 
        url: `/movie/popular?page=${page}` 
      });
      
      if (!isValidMoviesResponse(response)) {
        throw new Error('Invalid movies response');
      }
      
      return {
        movies: getMoviesAdapter(response.results),
        totalPages: response.total_pages
      };
    } catch (error) {
      console.error('Error fetching movies:', error);
      throw error;
    }
  };  