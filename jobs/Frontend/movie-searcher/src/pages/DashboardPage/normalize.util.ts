import type { RawMovieType } from "../../store/moviesSearch/types";

export const normalizeMoviesList = (moviesList?: RawMovieType[]) => {
  if (!moviesList) return [];
  return moviesList?.map(({ id, title, overview, poster_path }) => ({
    id,
    title,
    overview,
    imgUrl: poster_path,
  }));
};
