import { Movie } from "@/types/movie";
import { api } from "../remote";
import { getRemoteResults } from "../remote/response/data";
import { CategoryType, MovieType } from "../types/type";

export const moviesQuery = {
  key: (type: MovieType) => [`${CategoryType.Movie}/${type}`],
  fnc: (type: MovieType) =>
    api.get(`${CategoryType.Movie}/${type}`).then(getRemoteResults<Movie>),
};
