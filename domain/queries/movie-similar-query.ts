import { Movie } from "@/types/movie";
import { api } from "../remote";
import { getAxiosData, getRemoteResults } from "../remote/response/data";

const MOVIE_SIMILAR_QUERY_KEY = "movie-similar";

export const movieSimilarQuery = {
  key: (movieId: string) => [`${MOVIE_SIMILAR_QUERY_KEY}/${movieId}`],
  fnc: (movieId: string) =>
    api.get(`movie/${movieId}/similar`).then(getRemoteResults<Movie>),
};
