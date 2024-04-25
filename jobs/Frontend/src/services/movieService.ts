import { MovieSearchResult, Movie } from "@/types";
import { MovieSearchResponseAdapter } from "./adapters/MovieSearchResponseAdapter";
import { MovieByIdAdapter } from "./adapters";
import { CLIENT_API_URL } from "@/constants";

type MovieServiceOptions = {
  baseUrl: string;
  apiKey: string | undefined;
};

interface IMovieService {
  search: (searchTerm: string, page: number, options?: MovieServiceOptions) => Promise<MovieSearchResult>;
  getById: (id: number) => Promise<Movie>;
}
export const movieService: IMovieService = {
  async search(searchTerm, page, options = {
    baseUrl: CLIENT_API_URL,
    apiKey: undefined
  }) {
    let url = `${options.baseUrl}?query=${searchTerm}&page=${page}`;

    if (options.apiKey) {
      url = `${url}&api_key=${options.apiKey}`
    }

    const response = await fetch(url);
    return MovieSearchResponseAdapter(await response.json());
  },
  async getById(id: number) {
    const response = await fetch(`https://api.themoviedb.org/3/movie/${id}?api_key=d716410ceb4a322cd5e4df97906f2a6f`);
    const json = await response.json();
    return MovieByIdAdapter(json);
  }
}
