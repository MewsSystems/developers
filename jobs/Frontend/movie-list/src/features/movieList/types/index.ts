export type MovieListItem = {
  "adult": Boolean,
  "backdrop_path": String,
  "genre_ids": Number[],
  "id": Number,
  "original_language": String,
  "original_title": String,
  "overview": String,
  "popularity": Number,
  "poster_path": String,
  "release_date": String,
  "title": "Cookies",
  "video": Boolean,
  "vote_averate": Number,
  "vote_count": 53,
}

export type MovieList = MovieListItem[]

export type MovieListSearchQueryParams = {
  query: String,
  page?: Number,
  language?: String,
  "include_adult"?: Boolean,
  region?: String,
  year?: Number,
  "primary_release_year"?: Number
}