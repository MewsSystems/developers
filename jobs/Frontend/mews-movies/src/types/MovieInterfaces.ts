export interface MoviesResponse {
  results: Movie[];
  total_pages: number;
  total_results: number;
}

export interface Movie {
  id: number;
  title: string;
  release_date: string;
  poster_path: string;
  vote_average: number;
}

export interface Genre {
  id: number;
  name: string;
}

export interface CastMember {
  cast_id: number;
  character: string;
  name: string;
}

export interface CrewMember {
  job: string;
  name: string;
}

export interface Credits {
  cast: CastMember[];
  crew: CrewMember[];
}

export interface SpokenLanguage {
  english_name: string;
}

export interface MovieDetails extends Movie {
  vote_count: number;
  overview: string;
  genres: Genre[];
  credits: Credits;
  spoken_languages: SpokenLanguage[];
}
