import { Image } from "@/types/image";
import { api } from "../remote";
import { getAxiosData } from "../remote/response/data";

const MOVIE_IMAGES_QUERY_KEY = "movie-images";

export const movieImagesQuery = {
  key: (movieId: string) => [`${MOVIE_IMAGES_QUERY_KEY}/${movieId}`],
  fnc: (movieId: string) =>
    api
      .get(`movie/${movieId}/images`)
      .then(getAxiosData<{ backdrops: Image[] }>)
      .then((data) => data.backdrops.slice(0, 10)),
};
