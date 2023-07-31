export interface MovieDetailProps {
  id: number;
  isAdultFilm: boolean;
  collections: string;
  orginalLanguage: string;
  originalTitle: string;
  title: string;
  tagline: string;
  posterPath: string;
  budget: number;
  revenue: number;
  productionCompany: string;
  releaseDate: Date;
}
