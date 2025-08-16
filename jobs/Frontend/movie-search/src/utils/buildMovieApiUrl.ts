export const buildMovieApiUrl = (
  endpoint: string,
  query?: string,
  page = 1
) => {
  const baseUrl = process.env.MOVIE_DB_API_BASE_URL;
  const apiKey = process.env.MOVIE_DB_API_KEY;
  const queryParam = query ? `&query=${query}` : "";

  let url = `${baseUrl}${endpoint}?api_key=${apiKey}&include_adult=false&language=en-US&page=${page}${queryParam}`;

  return url;
};
