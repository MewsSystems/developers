export type Movie = {
  id: number;
  title: string;
  overview: string;
  posterImage: string;
};

export type MovieSearchResult = {
  page: number;
  totalPage: number;
  movies: Movie[];
}