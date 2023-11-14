import { Actor } from "@/types/actor";
import { api } from "../remote";
import { getAxiosData } from "../remote/response/data";
import { Detail } from "@/types/detail";

const ACTORS_QUERY_KEY = "actors";

export const actorsQuery = {
  key: (id: string, type: Detail) => [`${ACTORS_QUERY_KEY}/${id}/${type}`],
  fnc: (id: string, type: Detail) =>
    api
      .get(`${type}/${id}/credits`)
      .then(getAxiosData<{ cast: Actor[] }>)
      .then((data) => data.cast.slice(0, 5)),
};
