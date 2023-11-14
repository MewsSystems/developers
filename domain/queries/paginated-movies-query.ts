import { Movie } from "@/types/movie";
import { api } from "../remote";
import { Data, getAxiosData } from "../remote/response/data";
import { CategoryType, MovieType } from "../types/type";
import queryString from "query-string";

type Params = {
  page?: number;
  primary_release_year?: number;
  query?: string;
};

export const paginatedMoviesQuery = {
  key: (type: MovieType, params: Params) => [
    `${CategoryType.Movie}/${type}`,
    params,
  ],
  fnc: (type: MovieType, params: Params) => {
    const path =
      params.primary_release_year || params.query
        ? `${params.query ? "search" : "discover"}/${CategoryType.Movie}`
        : `${CategoryType.Movie}/${type}`;

    console.log(path);
    console.log(params);

    return api
      .get(`${path}?${queryString.stringify(params)}`)
      .then(getAxiosData<Data<Movie>>);
  },
};
