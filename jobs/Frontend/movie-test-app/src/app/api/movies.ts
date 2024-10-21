const movieDBApiKey = import.meta.env.VITE_MOVIE_DB_API_KEY;
const baseUrl = import.meta.env.VITE_MOVIE_DB_BASE_URL;
const apiVersion = import.meta.env.VITE_MOVIE_DB_API_VERSION;

const getMovies = async (search: string) => {
  const response = await fetch(
    `${baseUrl}/${apiVersion}/search/movie?api_key=${movieDBApiKey}&query=${search}`
  );
  return response.json();
};

export { getMovies };