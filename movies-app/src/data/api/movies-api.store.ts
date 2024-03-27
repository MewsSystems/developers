import {injectable} from 'inversify';
import { MoviesResponse, moviesResponseTypeguard, movieTypeguard } from "../types";

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
    }>): Promise<MoviesResponse> {
        // todo: dispose fetch in progress if not needed ?
        return fetch(
            `${SEARCH_ENDPOINT}?query=${query}&include_adult=${includeAdult}&language=${language}&page=${page}&api_key=${API_KEY}`,
            {
                headers: {
                    Accept: 'application/json',
                },
            }
        )
            .then(response => response.json())
            .then(data => new Promise((resolve, reject) => {
                moviesResponseTypeguard(data) ? resolve(data) : reject(new Error('Invalid data'));
            }));
    }
}