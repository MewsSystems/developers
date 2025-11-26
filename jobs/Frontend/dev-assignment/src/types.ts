export interface Movie {
  id: number;
  title: string;
  poster_path: string;
  overview: string;
  release_date: string;
}

export interface MovieDetail extends Movie {
  genres: { name: string }[];
  runtime: number;
  tagline: string;
}