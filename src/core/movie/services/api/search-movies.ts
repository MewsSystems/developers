import { movieApi } from "./movie-api";
import { getMoviesAdapter } from "../adapter/get-movies-adapter";
import { isValidMoviesResponse } from "./validation";
import { MoviesDataList } from "./types/movie-data-list";

export const searchMovies = async (query: string, page: number = 1): Promise<MoviesDataList> => {
    try {
      const response = await movieApi.get({ 
        url: `/search/movie?query=${encodeURIComponent(query)}&page=${page}` 
      });
      
      if (!isValidMoviesResponse(response)) {
        throw new Error('Invalid search response');
      }
      
      return {
        movies: getMoviesAdapter(response.results),
        totalPages: response.total_pages
      };
    } catch (error) {
      console.error('Error searching movies:', error);
      throw error;
    }
  };