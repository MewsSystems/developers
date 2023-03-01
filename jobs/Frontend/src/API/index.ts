import { 
  MovieListResult, 
  SearchMovieResult,
  Genre,
  ProductionCompany,
  ProductionCountry,
  SpokenLanguage,
  Movie,
} from "./interfaces";
import { Api } from './api';

const tmdbApi = new Api('03b8572954325680265531140190fd2a', 'https://api.themoviedb.org/3');

export type { 
  MovieListResult,
  SearchMovieResult,
  Genre,
  ProductionCompany,
  ProductionCountry,
  SpokenLanguage,
  Movie,
};

export default tmdbApi;
