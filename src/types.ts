export interface MovieListItem {
  adult: boolean
  backdropPath: string
  genreIds: string[]
  id: number
  originalLanguage: string
  originalTitle: string
  overview: string
  popularity: number
  posterPath: string
  releaseDate: string
  title: string
  video: boolean
  voteAverage: number
  voteCount: number
}

export interface MovieListQueryResponse {
  page: number
  results: MovieListItem[]
  total_pages: number
  total_results: number
}

interface Genre {
  id: number
  name: string
}

interface ProductionCompany {
  id: number
  logoPath: string
  name: string
  originCountry: string
}

interface ProductionCountry {
  iso31661: string
  name: string
}

interface SpokenLanguage {
  englishName: string
  iso6391: string
  name: string
}

export interface Movie {
  adult: boolean
  backdropPath: string
  belongsToCollection: {
    backdropPath: string
    id: number
    name: string
    posterPath: string
  }
  budget: number
  genres: Genre[]
  homepage: string
  id: number
  imdbId: string
  originalLanguage: string
  originalTitle: string
  overview: string
  popularity: number
  posterPath: string
  productionCompanies: ProductionCompany[]
  productionCountries: ProductionCountry[]
  releaseDate: string
  revenue: number
  runtime: number
  spokenLanguages: SpokenLanguage[]
  status: string
  tagline: string
  title: string
  video: boolean
  voteAverage: number
  voteCount: number
}
