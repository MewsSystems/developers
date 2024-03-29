import {injectable} from 'inversify';
import {
    movieFromMovieResponse, moviePageFromMoviesResponse,
    MovieResponse,
    movieResponseTypeguard,
    MoviesResponse,
    moviesResponseTypeguard,
} from "./types";
import { from, map, Observable } from "rxjs";
import { Movie, MoviesPage } from "../types";

const API_KEY = '03b8572954325680265531140190fd2a';
const SEARCH_ENDPOINT = 'https://api.themoviedb.org/3/search/movie';

// TODO: should be accessible only to other stores
@injectable()
export class MoviesApi {
    search({
        query,
        page = 1,
        language = 'en-US', // todo: enum with locals
        includeAdult = false,
    }: Readonly<{
        query: string;
        page?: number;
        language?: string;
        includeAdult?: boolean;
    }>): Observable<MoviesPage> {
        // todo: dispose fetch in progress if not needed ?
        return from(this.getData(
            `${SEARCH_ENDPOINT}?query=${query}&include_adult=${includeAdult}&language=${language}&page=${page}&api_key=${API_KEY}`,
            moviesResponseTypeguard
        )).pipe(
            map(moviePageFromMoviesResponse),
        );
    }

    // NB!
    // In fact detailed movie info differs from search result movie info
    // and have more info, like instead of genre_ids it has genres with names
    // but until we need this info we can use the same type
    getMovieInfo(movieId: number): Observable<Movie> {
        return from(this.getData(
            `https://api.themoviedb.org/3/movie/${movieId}?api_key=${API_KEY}`,
            movieResponseTypeguard,
        )).pipe(
            map(movieFromMovieResponse)
        );
    }

    private getData<T>(url: string, tepeguard: (value: unknown) => value is T): Promise<T> {
        // todo: dispose fetch in progress if not needed ?
        return fetch(url, { headers: { Accept: 'application/json' }})
            .then(response => response.json())
            .then(data => new Promise((resolve, reject) => {
                tepeguard(data) ? resolve(data) : reject(new Error(`Data not passing typeguard: ${JSON.stringify(data)}`));
            }));
    }
}