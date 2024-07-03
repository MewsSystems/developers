interface BaseMovie {
  id: number;
  posterUrl: string | null;
  releaseDate: string;
  title: string;
}

export interface MovieGenre {
  id: number;
  name: string;
}

export interface ProductionCompany {
  id: number;
  name: string;
  originCountry: string;
}

export interface MovieSearch extends BaseMovie {}

export interface MovieDetail extends BaseMovie {
  genres: MovieGenre[];
  originalLanguage: string;
  originalTitle: string;
  overview: string;
  productionCompanies: ProductionCompany[];
  runtime: number;
  status: string;
  tagline: string;
}
