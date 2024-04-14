export interface MovieDbSearchMoviesDto {
    page: number;
    total_pages: number;
    total_results: number;
    results: MovieDbSearchItemDto[];
}

export interface MovieDbSearchItemDto {
    adult: boolean;
    backdrop_path: string;
    genre_ids: number[];
    id: number;
    original_language: string;
    original_title: string;
    overview: string;
    popularity: string;
    poster_path: string;
    title: string;
    video: boolean;
    vote_average: number;
    vote_count: number;
}