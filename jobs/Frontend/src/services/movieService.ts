import { Movie, MovieSearchResult } from "@/types";
import { MovieSearchResponseAdapter } from "./adapters/MovieSearchResponseAdapter";
import { MovieByIdAdapter } from "./adapters";

interface IMovieService {
  search: (searchTerm: string, page: number) => Promise<MovieSearchResult>;
  getById: (id: number) => Promise<Movie>;
}
export const movieService: IMovieService = {
  async search(searchTerm, page) {
    const response = await fetch(`/api/search?query=${searchTerm}&page=${page}`);
    return MovieSearchResponseAdapter(await response.json());
  },
  async getById(id: number) {
    const response = await fetch(`https://api.themoviedb.org/3/movie/${id}?api_key=d716410ceb4a322cd5e4df97906f2a6f`);
    return MovieByIdAdapter(await response.json());
  }
}
