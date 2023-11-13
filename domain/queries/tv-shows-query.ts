import { TvShow } from "@/types/tv-show";
import { api } from "../remote";
import { getRemoteResults } from "../remote/response/data";
import { CategoryType, TvType } from "../types/type";

export const tvShowsQuery = {
  key: (type: TvType) => [`${CategoryType.Tv}/${type}`],
  fnc: (type: TvType) =>
    api.get(`${CategoryType.Tv}/${type}`).then(getRemoteResults<TvShow>),
};
