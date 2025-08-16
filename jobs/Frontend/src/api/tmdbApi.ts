import { TmdbDto } from '../interfaces/tmdbDto';
import { MovieMapper } from '../mapping/movieMapper';
import config from '../config.json';
import { MovieModel } from '../interfaces/movieModel';

const createTmdbQueryBase = () => new URLSearchParams([
    ['api_key', config.tmdb.apiKey]
]);

// docs: https://developer.themoviedb.org/reference/movie-details
export const getMovieDetail = async (movieId: number): Promise<MovieModel.MovieDetail> => {
    const requestQuery = createTmdbQueryBase();

    const response = await fetch(`${config.tmdb.apiBaseUrl}/3/movie/${movieId}?${requestQuery}`);
    if (response.status !== 200) {
        throw new Error('Invalid response');
    }

    const responseDto = await response.json() as TmdbDto.MovieDetail;

    return MovieMapper.movieDetailFromDto(responseDto);
};

// docs: https://developer.themoviedb.org/reference/movie-credits
export const getMovieCredits = async (movieId: number): Promise<MovieModel.MovieCredits> => {
    const requestQuery = createTmdbQueryBase();

    const response = await fetch(`${config.tmdb.apiBaseUrl}/3/movie/${movieId}/credits?${requestQuery}`);
    if (response.status !== 200) {
        throw new Error('Invalid response');
    }

    const responseDto = await response.json() as TmdbDto.MovieCredits;

    return MovieMapper.movieCreditsFromDto(responseDto);
};

// docs: https://developer.themoviedb.org/reference/search-movie
export const searchMovies = async (query: string, page: number = 1): Promise<MovieModel.MovieList> => {
    const requestQuery = createTmdbQueryBase();
    requestQuery.set('query', query);
    requestQuery.set('page', page.toString());

    const response = await fetch(`${config.tmdb.apiBaseUrl}/3/search/movie?${requestQuery}`);
    if (response.status !== 200) {
        throw new Error('Invalid response');
    }

    const responseDto = await response.json() as TmdbDto.MovieList;

    return MovieMapper.movieListFromDto(responseDto);
};

// docs: https://developer.themoviedb.org/reference/trending-movies
export const trendingMovies = async (timeWindow: 'day' | 'week', page: number = 1): Promise<MovieModel.MovieList> => {
    const requestQuery = createTmdbQueryBase();
    requestQuery.set('page', page.toString());

    const response = await fetch(`${config.tmdb.apiBaseUrl}/3/trending/movie/${timeWindow}?${requestQuery}`);
    if (response.status !== 200) {
        throw new Error('Invalid response');
    }

    const responseDto = await response.json() as TmdbDto.MovieList;

    return MovieMapper.movieListFromDto(responseDto);
};