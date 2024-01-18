export interface AllMovies {
  poster_path: string;
  original_title: string;
  release_date: string;
  vote_average: number;
  id: number;
}

export interface MoviesDetailsFull extends AllMovies {
  genre_ids: number;
  overview: string;
}
