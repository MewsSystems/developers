import { TmdbDto } from '../interfaces/tmdbDto.ts';
import { MovieMapper } from '../mapping/movieMapper.ts';

// working with images https://developer.themoviedb.org/docs/image-basics

// TODO: add comment that this can come from a config / ENV
const movieDbApiKey = '03b8572954325680265531140190fd2a';
const movieDbBaseUrl = 'https://api.themoviedb.org';

// TODO: add move details - docs: https://developer.themoviedb.org/reference/movie-details

// docs: https://developer.themoviedb.org/reference/search-movie
export const searchMovies = async (query: string) => {
    const requestQuery = new URLSearchParams([
        ['query', query],
        ['api_key', movieDbApiKey]
    ]);

    const response = await fetch(`${movieDbBaseUrl}/3/search/movie?${requestQuery}`);
    if (response.status !== 200) {
        throw new Error('Invalid response');
    }

    const responseDto = await response.json() as TmdbDto.SearchMovies;

    return MovieMapper.searchFromDto(responseDto);
};