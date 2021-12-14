export declare type MovieDetails = {
    title: string;
    tagline?: string;
    overview?: string;
    runtime?: number;
    status: string;
    poster_path?: string;
    [rest: string]: any;
};
export declare type MovieListItem = {
    title: string;
    id: number;
    release_date: string;
    vote_average: number;
    [rest: string]: any;
};
