export interface SearchResponse {
  page: number
  results: Result[]
  total_pages: number
  total_results: number
}

export interface Result {
  adult: boolean
  backdrop_path: null | string
  genre_ids: number[]
  id: number
  original_language: OriginalLanguage
  original_title: string
  overview: string
  popularity: number
  poster_path: string
  release_date: string
  title: string
  video: boolean
  vote_average: number
  vote_count: number
}

export enum OriginalLanguage {
  En = "en",
  Fr = "fr",
  Ja = "ja",
}
