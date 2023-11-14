import { Movie } from "@/types/movie";
import { api } from "../remote";
import { getRemoteResults } from "../remote/response/data";
import { Detail } from "@/types/detail";
import { TvShow } from "@/types/tv-show";

const SIMILAR_QUERY_KEY = "similar";

export const similarQuery = {
  key: (id: string, type: Detail) => [`${SIMILAR_QUERY_KEY}/${id}/${type}`],
  fnc: (id: string, type: Detail) =>
    api.get(`${type}/${id}/similar`).then(getRemoteResults<Movie | TvShow>),
};
