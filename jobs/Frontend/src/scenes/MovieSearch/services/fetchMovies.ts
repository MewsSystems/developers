import { MovieSearchResult } from "@/scenes/MovieSearch/services/types";

/**
 * Fetches movies from TMDB API
 * @param query - The search query which will be used to search for movies by title
 * @param page - The page number to fetch
 */
const fetchMovies = async (
  query: string,
  page: number,
): Promise<MovieSearchResult> => {
  const res = await fetch(
    `https://api.themoviedb.org/3/search/movie?api_key=${process.env.NEXT_PUBLIC_MOVIES_API_KEY}&query=${query}&page=${page}`,
  );
  return res.json();
};

export default fetchMovies;
