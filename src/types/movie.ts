export type MovieCard = {
    id: number;
    title: string;
    posterPath: string;
  };

export type MovieApiItem = {
    id: number;
    title: string;
    poster_path: string;
    [key: string]: any;
  };

export type MovieApiResponse = {
    page: number;
    results: MovieApiItem[];
    total_pages: number;
    total_results: number;
};

export type MovieCardContainerProps = {
  page: number;
  items: MovieCard[];
};