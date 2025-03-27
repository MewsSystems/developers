export type Genre = {
    id: number;
    name: string;
};
  
export type MovieDetailApiItem = {
    title?: string;
    tagline?: string | null;
    backdrop_path?: string | null;
    vote_average?: number | null;
    release_date?: string | null;
    original_language?: string | null;
    runtime?: number | null;
    homepage?: string | null;
    overview?: string | null;
    genres?: Genre[];
};
  
export type MovieDetailInfo = {
    title?: string;
    tagline?: string;
    backdropPath?: string;
    genres?: string;
    userScore?: string;
    releaseDate?: string;
    originalLanguage?: string;
    runtime?: string;
    homepage?: string;
    overview?: string;
};