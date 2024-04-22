export type TMDBResolution = "w600_and_h900" | "w300_and_h450";

export function getImageURL(resolution: TMDBResolution, path: string) {
  return `https://media.themoviedb.org/t/p/${resolution}_bestv2${path}`;
}

export function getBlurredImageURL(path: string) {
  return `https://media.themoviedb.org/t/p/w300_and_h450_multi_faces_filter(blur)${path}`;
}

export function getTMDBMovieLink(id: string) {
  return `https://www.themoviedb.org/movie/${id}`;
}

export function getTMDBPersonLink(id: number) {
  return `https://www.themoviedb.org/person/${id}`;
}
