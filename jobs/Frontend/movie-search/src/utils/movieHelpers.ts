import type { Movie } from "../api/types";

export const getImageUrl = (
  imageUrl: string | null,
  size: string = "w500"
): string => {
  const BASE_IMG_URL = "https://image.tmdb.org/t/p";

  if (!imageUrl) {
    return "Image does not exist";
  }

  return `${BASE_IMG_URL}/${size}${imageUrl}`;
};

export const getYearFromDate = (date: string): string => {
  const releaseDate = new Date(date);
  return releaseDate.getFullYear().toString();
};

export const getTranslatedTitle = (
  isEnglish: boolean,
  originalTitle: string,
  translatedTitle: string
) => {
  if (isEnglish) {
    return originalTitle;
  } else {
    return `${originalTitle} (${translatedTitle})`;
  }
};

export const getMovieDetailRoute = (movieId: Movie["id"]): string => {
  return `/movie/${movieId}`;
};

export function formatRuntime(totalMinutes: number | null): string {
  if (totalMinutes == null || totalMinutes <= 0) return "N/A";
  const hours = Math.floor(totalMinutes / 60);
  const minutes = totalMinutes % 60;
  const paddedMins = minutes.toString().padStart(2, "0");
  return `${hours}h ${paddedMins}m`;
}
