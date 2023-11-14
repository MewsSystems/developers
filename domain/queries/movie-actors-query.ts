import { Actor } from "@/types/actor";
import { api } from "../remote";
import { getAxiosData } from "../remote/response/data";

const MOVIE_ACTORS_QUERY_KEY = "movie-actors";

export const movieActorsQuery = {
  key: (movieId: string) => [`${MOVIE_ACTORS_QUERY_KEY}/${movieId}`],
  fnc: (movieId: string) =>
    api
      .get(`movie/${movieId}/credits`)
      .then(getAxiosData<{ cast: Actor[] }>)
      .then((data) => data.cast.slice(0, 5)),
};
