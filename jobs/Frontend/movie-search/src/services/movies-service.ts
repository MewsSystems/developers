import wretch from "wretch";
import {
  GenereResponse,
  MovieDetailResponse,
  MovieItemResponse,
  MoviesPageResponse,
} from "./responses/movies-response";
import { MovieDetail, MovieItem, MoviesPage } from "../models/movies.types";

const API_KEY = "03b8572954325680265531140190fd2a";
const BASE_API_URL = "https://api.themoviedb.org/3/";

const api = wretch(BASE_API_URL, { mode: "cors" })
  .errorType("json")
  .resolve((r) => r.json());

export const searchMoviesEndpoint = (
  query: string,
  page: number
): Promise<MoviesPage> => {
  return api
    .get(`search/movie?api_key=${API_KEY}&query=${query}&page=${page}`)
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

export const getMovieDetailEndpoint = (
  movieId: number
): Promise<MovieDetail> => {
  return api
    .get(`movie/${movieId}?api_key=${API_KEY}`)
    .then<MovieDetailResponse>()
    .then((data: MovieDetailResponse) => {
      const moviepage: MovieDetail = {
        adult: data.adult,
        budget: data.budget,
        revenue: data.revenue,
        overview: data.overview,
        releaseDate: data.release_date,
        posterPath: data.poster_path,
        popularity: data.popularity,
        runtime: data.runtime,
        status: data.status,
        voteAverage: data.vote_average,
        voteCount: data.vote_count,
        genres: getGeneres(data.genres),
        id: data.id,
        originalLanguage: data.original_language,
        originalTitle: data.original_title,
        tagline: data.tagline,
        title: data.title,
      };

      return moviepage;
    });
};

const getGeneres = (genres: GenereResponse[]): string => {
  const generesNames = genres.map((value) => {
    return value.name;
  });

  return generesNames.toString();
};

const moviesItemMapper = (movieItem: MovieItemResponse): MovieItem => {
  return {
    id: movieItem.id,
    isAdultFilm: movieItem.adult,
    orginalLanguage: movieItem.original_language,
    originalTitle: movieItem.original_title,
    posterPath: movieItem.poster_path,
    releaseDate: movieItem.release_date?movieItem.release_date:'TBD',
    title: movieItem.title,
  };
};

const resultMapper = (results: MovieItemResponse[]): MovieItem[] => {
  return results.map((item) => {
    return moviesItemMapper(item);
  });
};
