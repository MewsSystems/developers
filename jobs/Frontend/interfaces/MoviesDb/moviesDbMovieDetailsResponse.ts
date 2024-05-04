interface MovieGenre {
  id: number;
  name: string;
}

interface ProductionCompany {
  id: number;
  name: string;
  origin_country: string;
}

export interface MoviesDbMovieDetailsResponse {
  budget: number;
  genres: MovieGenre[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  poster_path: string | null;
  production_companies: ProductionCompany[];
  release_date: string;
  revenue: number;
  runtime: number;
  status: string;
  tagline: string;
  title: string;
}
