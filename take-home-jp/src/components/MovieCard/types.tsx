export type Genre = {
  id: number;
  name: string;
};

export type Movie = {
  id: number;
  overview: string;
  favourite_count: number;
  poster_path: string;
  title: string;
  backdrop_path: string;
  vote_average: number;
  vote_count: number;
  release_date: string;
  runtime?: number;
  genres: Genre[];
};
