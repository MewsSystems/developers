export const getImageURL = (path: string, size: string = "w300") => {
  return `${import.meta.env.VITE_MOVIE_DB_IMAGE_BASE_URL}${size}${path}`;
};
