import { TmdbDto } from '../interfaces/tmdbDto.ts';
import { MovieMapper } from '../mapping/movieMapper.ts';
import config from '../config.json';

const createTmdbQueryBase = () => new URLSearchParams([
    ['api_key', config.tmdb.apiKey]
]);

// docs: https://developer.themoviedb.org/reference/movie-details
export const getMovieDetail = async (movieId: number) => {
    const requestQuery = createTmdbQueryBase();

    const response = await fetch(`${config.tmdb.apiBaseUrl}/3/movie/${movieId}?${requestQuery}`);
    if (response.status !== 200) {
        throw new Error('Invalid response');
    }

    const responseDto = await response.json() as TmdbDto.MovieDetail;

    return MovieMapper.movieDetailFromDto(responseDto);
};

// docs: https://developer.themoviedb.org/reference/search-movie
export const searchMovies = async (query: string, page: number = 1) => {
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
export const trendingMovies = async (timeWindow: 'day' | 'week', page: number = 1) => {
    const requestQuery = createTmdbQueryBase();
    requestQuery.set('page', page.toString());

    const response = await fetch(`${config.tmdb.apiBaseUrl}/3/trending/movie/${timeWindow}?${requestQuery}`);
    if (response.status !== 200) {
        throw new Error('Invalid response');
    }

    const responseDto = await response.json() as TmdbDto.MovieList;

    return MovieMapper.movieListFromDto(responseDto);
};