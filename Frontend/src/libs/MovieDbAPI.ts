import { Api, DefaultApiException, DefaultResponseProcessor } from 'rest-api-handler';

export interface Movie {
    id: number,
    title: string,
    overview: string,
    release_date: string,
}

export interface MovieDetail extends Movie {
    poster_path: string | null,
}

export interface SearchResponse {
    page: number,
    total_results: number,
    total_pages: number,
    results: Movie[],
}

export default class MovieDbAPI extends Api {
    apiKey: string;

    constructor(apiKey: string) {
        super(
            'https://api.themoviedb.org/3',
            [
                new DefaultResponseProcessor(DefaultApiException),
            ], {
                'Content-Type': 'application/json;charset=utf-8',
            },
        );

        this.apiKey = apiKey;
    }

    async getMovie(id: number): Promise<MovieDetail> {
        const { data } = await this.get(`movie/${id}?api_key=${this.apiKey}`);
        return data;
    }

    async search(query: string, page = 1): Promise<SearchResponse> {
        const { data } = await this.get(`search/movie?api_key=${this.apiKey}&query=${query}&page=${page}`);
        return data;
    }
}
