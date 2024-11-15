import { useQuery } from "@tanstack/react-query";
import { apiKey, movieDetailsEndpoint } from "../../config";
import { QueryOptionsWithoutKeyAndFn } from "types";
import { Movies } from "@/MovieSearch/useGetMovies";

export type Movie = Partial<{
  adult: boolean;
  backdrop_path: string;
  belongs_to_collection: string;
  budget: number;
  genres: Array<
    Partial<{
      id: number;
      name: string;
    }>
  >;
  homepage: string;
  imdb_id: string;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  production_companies: Array<
    Partial<{
      id: number;
      logo_path: string;
      name: string;
      origin_country: string;
    }>
  >;
  production_countries: Array<
    Partial<{
      iso_3166_1: string;
      name: string;
    }>
  >;
  release_date: string;
  revenue: number;
  runtime: number;
  spoken_languages: Array<
    Partial<{
      english_name: string;
      iso_639_1: string;
      name: string;
    }>
  >;
  status: string;
  tagline: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}> & {
  id: number;
};

const fetchMovie = async (movieId: number): Promise<Movie> => {
  const response = await fetch(`${movieDetailsEndpoint}${movieId}?api_key=${apiKey}`);

  if (!response.ok) {
    throw new Error("Failed to fetch a movie!");
  }

  return response.json();
};

type UseGetMovieProps = QueryOptionsWithoutKeyAndFn<Movies> & {
  movieId: number;
};

export const useGetMovie = ({ movieId }: UseGetMovieProps) =>
  useQuery<Movie>({
    queryKey: ["movies", movieId],
    queryFn: () => fetchMovie(movieId),
  });
