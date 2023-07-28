import { MovieItem, MoviesPage } from "../store/movie-slice";
import wretch from "wretch";
import { MovieItemResponse, MoviesPageResponse } from "../api/movies-response";

const API_KEY = "03b8572954325680265531140190fd2a";
const BASE_API_URL = "https://api.themoviedb.org/3/";

const api = wretch(BASE_API_URL, { mode: "cors" })
  .errorType("json")
  .resolve((r) => r.json());

export const searchMoviesEndpoint = (query: string): Promise<MoviesPage> => {
  return api
    .get(`search/movie?api_key=${API_KEY}&query=${query}`)
    .then<MoviesPageResponse>()
    .then((data: MoviesPageResponse) => {
      const moviepage: MoviesPage = {
        page: data.page,
        totalPages: data.total_pages,
        total: data.total_results,
        movies: resultMapper(data.results),
      };

      return moviepage;
    });
};

const moviesItemMapper = (movieItem: MovieItemResponse): MovieItem => {
  return {
    id: movieItem.id,
    isAdultFilm: movieItem.adult,
    orginalLanguage: movieItem.original_language,
    originalTitle: movieItem.original_title,
    posterPath: movieItem.poster_path,
    releaseDate: movieItem.release_date,
    title: movieItem.title,
  };
};

const resultMapper = (results: MovieItemResponse[]): MovieItem[] => {
  return results.map((item) => {
    return moviesItemMapper(item);
  });
};
