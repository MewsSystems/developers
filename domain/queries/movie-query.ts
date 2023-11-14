import { Movie } from "@/types/movie";
import { api } from "../remote";
import { getAxiosData } from "../remote/response/data";
import { CategoryType } from "../types/type";

export const movieQuery = {
  key: (id: string) => [`${CategoryType.Movie}/${id}`],
  fnc: (id: string) =>
    api.get(`${CategoryType.Movie}/${id}`).then(getAxiosData<Movie>),
};
