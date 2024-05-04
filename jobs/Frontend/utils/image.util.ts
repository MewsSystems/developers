export const getImageUrl = (size: number, url: string) => {
  return `${process.env.NEXT_PUBLIC_MOVIE_DB_API_IMAGE_BASE_URL}/w${size}/${url}`;
};
