import { TvShow } from "@/types/tv-show";
import { api } from "../remote";
import { getAxiosData } from "../remote/response/data";
import { CategoryType } from "../types/type";

export const tvShowQuery = {
  key: (id: string) => [`${CategoryType.Tv}/${id}`],
  fnc: (id: string) =>
    api.get(`${CategoryType.Tv}/${id}`).then(getAxiosData<TvShow>),
};
