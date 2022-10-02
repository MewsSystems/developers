// API fetch results
interface IFetchData {
  page: number,
  results: IFetchMovieItem[],
  total_pages: number,
  total_results: number
}

// Single API fetch result
interface IFetchMovieItem {
  id: number,
  title: string,
  release_date: string,
  poster_path?: string,
  vote_count: number,
  vote_average: number,
  original_language: string,
  original_title: string
}

// Movie endpoint API fetch result with additional data
interface IFetchMovieDetail extends IFetchMovieItem {
  tagline?: string,
  backdrop_path?: string,
  overview: string,
  budget: number,
  genres: [{
    name: string
  }],
  revenue: number
}

interface ILangs {
  [key: string]: string
}

declare module 'react-scroll';
