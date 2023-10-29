export type Movie = {
  adult: boolean;
  backdrop_path: string;
  id: number;
  title: string;
  original_language: string;
  original_title: string;
  overview: string;
  poster_path: string;
  media_type: string;
  genre_ids: number[];
  popularity: number;
  release_date: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};

export type Person = {
  adult: boolean;
  gender: number;
  id: number;
  known_for_department: string;
  name: string;
  known_for: Array<{
    adult: boolean;
    backdrop_path: string;
    id: number;
    title?: string;
    name?: string;
  }>;
  original_name?: string;
  popularity: number;
  profile_path: string;
};

export type SearchResponse = {
  page: number;
  total_results: number;
  total_pages: number;
  results: Movie[] | Person[];
};
