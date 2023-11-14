import { Image } from "@/types/image";
import { api } from "../remote";
import { getAxiosData } from "../remote/response/data";
import { Detail } from "@/types/detail";

const IMAGES_QUERY_KEY = "images";

export const imagesQuery = {
  key: (id: string, type: Detail) => [`${IMAGES_QUERY_KEY}/${id}/${type}`],
  fnc: (id: string, type: Detail) =>
    api
      .get(`${type}/${id}/images`)
      .then(getAxiosData<{ backdrops: Image[] }>)
      .then((data) => data.backdrops.slice(0, 10)),
};
