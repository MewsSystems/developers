export type MovieListItem = {
  "adult": Boolean,
  "backdrop_path": string,
  "genre_ids": Number[],
  "id": Number,
  "original_language": string,
  "original_title": string,
  "overview": string,
  "popularity": Number,
  "poster_path": string,
  "release_date": string,
  "title": "Cookies",
  "video": Boolean,
  "vote_averate": Number,
  "vote_count": 53,
}

export type MovieList = MovieListItem[]

export type MovieListQueryResult = {
  "total_pages": Number,
  "total_results": Number,
  "results": MovieList
}

export type MovieListSearchQueryParams = {
  query: string,
  page?: Number,
  language?: string,
  "include_adult"?: Boolean,
  region?: string,
  year?: Number,
  "primary_release_year"?: Number
}