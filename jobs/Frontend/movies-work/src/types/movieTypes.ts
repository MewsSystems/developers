export interface IMovie {
  adult: boolean;
  backdrop_path: string;
  id: number;
  overview: string;
  popularity: number;
  releaseDate: string;
  status: string;
  title: string;
  voteAverage: number;
  voteCount: number;
  poster_path: string;
  release_date: string;
}

export interface IMoviesData {
  page: number;
  results: IMovie[];
  total_pages: number;
  total_results: number;
}
