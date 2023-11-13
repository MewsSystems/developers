export const apiConfig = {
  baseUrl: process.env.NEXT_PUBLIC_REMOTE_API_URL,
  apiKey: process.env.NEXT_PUBLIC_SCREENPLAY_API_KEY,
  coverImage: (imagePath: string) =>
    `${process.env.NEXT_PUBLIC_API_IMAGE_PATH}original${imagePath}`,
  posterImage: (imagePath: string) =>
    `${process.env.NEXT_PUBLIC_API_IMAGE_PATH}w500${imagePath}`,
};
