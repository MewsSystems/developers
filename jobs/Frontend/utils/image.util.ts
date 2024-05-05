export const getImageUrl = (size: number, url: string) => {
  if (url.startsWith("/")) {
    return `${process.env.NEXT_PUBLIC_MOVIE_DB_API_IMAGE_BASE_URL}/w${size}${url}`;
  }

  return `${process.env.NEXT_PUBLIC_MOVIE_DB_API_IMAGE_BASE_URL}/w${size}/${url}`;
};
