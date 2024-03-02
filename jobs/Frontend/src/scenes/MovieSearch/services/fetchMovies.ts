import { MovieSearchResult } from "@/scenes/MovieSearch/services/types";

const fetchMovies = async (query: string): Promise<MovieSearchResult> => {
  const res = await fetch(
    `https://api.themoviedb.org/3/search/movie?api_key=${process.env.NEXT_PUBLIC_MOVIES_API_KEY}&query=${query}`,
  );
  return res.json();
};

export default fetchMovies;
