import type { RawMovieType } from "../../store/moviesSearch/types";

export const normalizeMoviesList = (moviesList?: RawMovieType[]) => {
  if (!moviesList) return [];
  console.log("moviesList", moviesList);

  return moviesList?.map(({ id, title, overview, poster_path }) => ({
    id,
    title,
    overview,
    imgUrl: poster_path,
  }));
};
