import { SearchResponse } from "../models/search-response";
import { MovieDetails } from "../models/movie-details";

export class MoviesDBService {
    static getMovieById(id: string): Promise<MovieDetails> {
        const baseUrl = `${process.env.REACT_APP_API_URL}/movie/${id}`;
        const apiKey = process.env.REACT_APP_API_KEY;
        const url = `${baseUrl}?api_key=${apiKey}&language=en-US`;

        return fetch(url)
            .then(response => response.json())
    }

    static searchMovies(query: string, page: number): Promise<SearchResponse> {
        const baseUrl = `${process.env.REACT_APP_API_URL}/search/movie`;
        const apiKey = process.env.REACT_APP_API_KEY;
        const url = `${baseUrl}?api_key=${apiKey}&language=en-US&query=${query}&page=${page}&include_adult=false`;

        return fetch(url)
            .then(response => response.json())
    }
}
