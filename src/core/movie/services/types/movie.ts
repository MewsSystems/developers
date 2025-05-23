export interface Movie {
    id: number;
    title: string;
    overview: string;
    posterPath: string;
    releaseDate: string;
    voteAverage: number;
    voteCount: number;
    popularity: number;
    backdropPath: string | null;
    genreIds: number[];
    language: string;
    video: boolean;
    runtime?: number;
}