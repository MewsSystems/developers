export type MovieType = {
  id: number;
  title: string;
  poster_path?: string | null;
  vote_average: number;
};

export type MovieDetailType = {
  id: number;
  title: string;
  poster_path?: string | null;
  overview: string;
  release_date: string;
  vote_average: number;
};
