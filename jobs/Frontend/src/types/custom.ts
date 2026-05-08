// tmdb-types.ts

import { paths } from "./tmdb";

// Example: GET /movie/{movie_id}
export type MovieDetailsResponse =
  paths["/3/movie/{movie_id}"]["get"]["responses"]["200"]["content"]["application/json"];

// Example: GET /search/movie
export type MovieSearchResponse =
  paths["/3/search/movie"]["get"]["responses"]["200"]["content"]["application/json"];

export type MovieDetailProductionCountries =
  paths["/3/movie/{movie_id}"]["get"]["responses"]["200"]["content"]["application/json"]["production_countries"];

export type MovieDetailGenres =
  paths["/3/movie/{movie_id}"]["get"]["responses"]["200"]["content"]["application/json"]["genres"];
