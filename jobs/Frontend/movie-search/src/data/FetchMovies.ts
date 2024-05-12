import { BaseMovie, TMDBSearchResponse } from "@/interfaces/MovieInterfaces";
import { buildMovieApiUrl } from "../utils/buildMovieApiUrl";

export const fetchMovies = async (
  movieUrl: string,
  searchterm: string,
  page: number
) => {
  const movieApiUrl = buildMovieApiUrl(movieUrl, searchterm, page);
  const movieFetchOptions = {
    method: "GET",
    headers: { accept: "application/json" },
  };
  const MovieResponse = await fetch(movieApiUrl, movieFetchOptions);

  const movies = (await MovieResponse.json()) as TMDBSearchResponse<BaseMovie>;

  // we dont need all the data so pull what we need from it
  const MovieDataArray = movies.results.map((movie) => {
    return {
      id: movie.id,
      title: movie.title,
      overview: movie.overview,
      releaseDate: movie.release_date,
      posterPath: movie.poster_path,
    };
  });

  return {
    movies: MovieDataArray,
    totalPages: movies.total_pages,
  };
};
