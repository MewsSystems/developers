import {MovieDbSearchMoviesDto} from './interfaces/movieDbSearchMoviesDto';

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

    return await response.json() as MovieDbSearchMoviesDto;
};