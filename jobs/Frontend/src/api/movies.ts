import { MovieSearch } from "../redux/movies/movies.slice.types";
import { apiInstance } from "./instance";

export const MoviesAPI = {
  search: async (query: string, page: number) => {
    try {
      const response = await apiInstance.get<MovieSearch>("/search/movie", {
        params: {
          query,
          page,
        },
      });
      return response.data;
    } catch (error) {
      console.error(error);
    }
  },
  getDetails: async (id: number) => {
    try {
      const response = await apiInstance.get(`/movie/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  },
};
