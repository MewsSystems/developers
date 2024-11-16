import { Movie, MovieSearchResponse } from './types';

const API_KEY = '03b8572954325680265531140190fd2a';
const BASE_URL = 'https://api.themoviedb.org/3';

export const searchMovies = async (query: string, page: number = 1): Promise<MovieSearchResponse> => {
    const response = await fetch(
        `${BASE_URL}/search/movie?api_key=${API_KEY}&query=${encodeURIComponent(query)}&page=${page}`
    );
    if (!response.ok) {
        throw new Error('Failed to fetch movies');
    }
    return response.json();
};

export const getMovieDetails = async (movieId: number): Promise<Movie> => {
    const response = await fetch(
        `${BASE_URL}/movie/${movieId}?api_key=${API_KEY}`
    );
    if (!response.ok) {
        throw new Error('Failed to fetch movie details');
    }
    return response.json();
};