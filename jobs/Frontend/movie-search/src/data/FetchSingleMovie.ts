import { BaseMovieDetails } from "@/interfaces/MovieInterfaces";
import { buildMovieApiUrl } from "../utils/buildMovieApiUrl";

export const fetchSingleMovie = async (movieUrl: string, movideId: number) => {
  const movieApiUrl = buildMovieApiUrl(`${movieUrl}${movideId}`);
  const movieFetchOptions = {
    method: "GET",
    headers: { accept: "application/json" },
  };
  const movieResponse = await fetch(movieApiUrl, movieFetchOptions);

  const movieResponseData = (await movieResponse.json()) as BaseMovieDetails;

  return movieResponseData;
};
