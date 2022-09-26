export type MovieListItem = {
  "adult": Boolean,
  "backdrop_path": string,
  "genre_ids": number[],
  "id": number,
  "original_language": string,
  "original_title": string,
  "overview": string,
  "popularity": number,
  "poster_path": string,
  "release_date": string,
  "title": "Cookies",
  "video": Boolean,
  "vote_averate": number,
  "vote_count": 53,
}

export type MovieList = MovieListItem[]

export type MovieListQueryResult = {
  "total_pages": number,
  "total_results": number,
  "results": MovieList
}

export type MovieListSearchQueryParams = {
  query: string,
  page?: number,
  language?: string,
  "include_adult"?: Boolean,
  region?: string,
  year?: number,
  "primary_release_year"?: number
}