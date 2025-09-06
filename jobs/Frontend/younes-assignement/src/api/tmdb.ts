const API_KEY = import.meta.env.VITE_TMDB_API_KEY;
const API_URL = import.meta.env.VITE_TMDB_API_URL;

export const fetchMovies = async (query: string, page: number = 1) => {
  const res = await fetch(
    `${API_URL}/search/movie?api_key=${API_KEY}&query=${query}&page=${page}`,
  );
  return res.json();
};

export const fetchMovieDetail = async (id: string) => {
  const res = await fetch(`${API_URL}/movie/${id}?api_key=${API_KEY}`);
  return res.json();
};
