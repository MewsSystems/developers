import { IMAGE_BASE_URL } from '../../../const/image-base-url';

export const apiConfig = {
  baseUrl: 'https://api.themoviedb.org/3',
  apiKey: import.meta.env.VITE_TMDB_API_KEY,
  imageBaseUrl: IMAGE_BASE_URL,
};
