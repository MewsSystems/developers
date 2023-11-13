export const apiConfig = {
  baseUrl: process.env.NEXT_PUBLIC_REMOTE_API_URL,
  accessToken: process.env.NEXT_PUBLIC_ACCESS_TOKEN,
  coverImage: (imagePath: string) =>
    `${process.env.NEXT_PUBLIC_API_IMAGE_PATH}original${imagePath}`,
  posterImage: (imagePath: string) =>
    `${process.env.NEXT_PUBLIC_API_IMAGE_PATH}w500${imagePath}`,
};
