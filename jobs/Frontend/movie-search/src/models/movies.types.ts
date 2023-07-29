

export interface MovieItem {
    id: number;
    isAdultFilm: boolean;
    orginalLanguage: string;
    originalTitle: string;
    title: string;
    posterPath: string;
    releaseDate: string;
  }
  
  export interface MovieDetail {
    adult: boolean;
    budget: number;
    genres: string;
    id: number;
    originalLanguage: string;
    originalTitle: string;
    overview: string;
    popularity: number;
    posterPath: string;
    releaseDate: string;
    revenue: number;
    runtime: number;
    status: string;
    tagline: string;
    title: string;
    voteAverage: number;
    voteCount: number;
  }
  
  export interface MoviesPage {
    movies: MovieItem[];
    total: number;
    page: number;
    totalPages: number;
  }
  