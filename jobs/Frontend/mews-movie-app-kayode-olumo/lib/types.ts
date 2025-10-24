export type TmdbMovieSummary = {
  id: number;
  title: string;
  overview: string;
  poster_path: string | null;
  vote_average?: number;        
};

export type SearchResponse = {
  page: number;
  results: TmdbMovieSummary[];
  total_pages: number;
  total_results: number;
};

export type TmdbGenre = { id: number; name: string };

export type TmdbMovieDetail = {
  id: number;
  title: string;
  overview: string;
  poster_path: string | null;
  release_date?: string;
  vote_average?: number;
  runtime?: number | null;
  genres?: TmdbGenre[];
};

export type TmdbCast = {
  id: number;
  name: string;
  character?: string;
  profile_path?: string | null;
  order?: number;             
};

export type TmdbCrew = {
  id: number;
  name: string;
  job?: string;
  department?: string;
};

export type CreditsResponse = {
  id: number;
  cast: TmdbCast[];
  crew: TmdbCrew[];
};