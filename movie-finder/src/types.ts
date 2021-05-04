export type Genres = {
    id: number;
    name: string;
};

export type MovieDetail = {
    adult: boolean;
    budget: number;
    id: number;
    original_language: string;
    original_title: string;
    poster_path: string | null;
    release_date: string;
    revenue: number ;
    overview: string;
    backdrop_path: string;
    genres: Genres[] | undefined;
    vote_average: number;
    vote_count: number;
};

export type MovieDetailViewParams = { movieId: string };

export type Movie = {
    adult: boolean;
    backdrop_path: string;
    id: number;
    original_title: string;
    overview: string;
    poster_path: string;
    release_date: string;
    title: string;
    vote_average: number;
    vote_count: number;
};

export type MoviesPage={
    pageNumber: number,
    movies: Movie[],
    searchValue: string,
    totalPages: number,
}