import { api } from "../remote";
import { Data, getAxiosData } from "../remote/response/data";
import { CategoryType, TvType } from "../types/type";
import queryString from "query-string";
import { TvShow } from "@/types/tv-show";

type Params = {
  page?: number;
  primary_release_year?: number;
  query?: string;
};

export const paginatedTvShowsQuery = {
  key: (type: TvType, params: Params) => [`${CategoryType.Tv}/${type}`, params],
  fnc: (type: TvType, params: Params) => {
    const path =
      params.primary_release_year || params.query
        ? `${params.query ? "search" : "discover"}/${CategoryType.Tv}`
        : `${CategoryType.Tv}/${type}`;

    return api
      .get(`${path}?${queryString.stringify(params)}`)
      .then(getAxiosData<Data<TvShow>>);
  },
};
