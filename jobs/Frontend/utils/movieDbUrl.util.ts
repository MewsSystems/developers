export const constructMovieDbUrl = (
  endpoint: string,
  query?: string,
  page = 1,
) => {
  let url =
    `${process.env.MOVIE_DB_API_BASE_URL}${endpoint}` +
    `?api_key=${process.env.MOVIE_DB_API_KEY}` +
    `&include_adult=${process.env.MOVIE_DB_API_INCLUDE_ADULT}` +
    `&language=${process.env.MOVIE_DB_API_LANGUAGE}` +
    `&page=${page}`;

  if (query) {
    url += `&query=${query}`;
  }

  return url;
};
