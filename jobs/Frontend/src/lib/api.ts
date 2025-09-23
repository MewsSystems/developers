import { MovieDetailsResponse, MovieSearchResponse } from "../types/custom";

export const queryMovies = async (query: string, page: string) => {
  const url = `https://api.themoviedb.org/3/search/movie?query=${query}&page=${page}`;
  const response = await fetch(url, {
    method: "GET",
    headers: {
      Authorization:
        "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJlNTM0YTJkNWUxZmZmNjdkMGM4MWQzYjg2ODA0ODgwOSIsIm5iZiI6MTczMzE3NzYzMC4yODcwMDAyLCJzdWIiOiI2NzRlMzExZTIzZGY5NjBkYzRjZDc5NWUiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.rHYEKSdSO4TFea1YEaIsL2xXH3cL14JLZ6P6OJzvGtE",
    },
  });

  const result: MovieSearchResponse = await response.json();

  return result;
};

export const queryMovieDetails = async (movieId: string) => {
  const url = `https://api.themoviedb.org/3/movie/${movieId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: {
      Authorization:
        "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJlNTM0YTJkNWUxZmZmNjdkMGM4MWQzYjg2ODA0ODgwOSIsIm5iZiI6MTczMzE3NzYzMC4yODcwMDAyLCJzdWIiOiI2NzRlMzExZTIzZGY5NjBkYzRjZDc5NWUiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.rHYEKSdSO4TFea1YEaIsL2xXH3cL14JLZ6P6OJzvGtE",
    },
  });

  const result: MovieDetailsResponse = await response.json();

  return result;
};
