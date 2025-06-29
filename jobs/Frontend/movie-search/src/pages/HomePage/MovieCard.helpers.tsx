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
