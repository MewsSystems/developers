export const API_KEY = import.meta.env.VITE_TMDB_API_KEY;
export const ENDPOINT_URL_BASE = 'https://api.themoviedb.org/3';
export const ENDPOINT_URL_IMAGES_w92 = 'https://image.tmdb.org/t/p/w92';
export const ENDPOINT_URL_IMAGES_w500 = 'https://image.tmdb.org/t/p/w500';
export const PRIVACY_POLICY_LINK = 'https://www.themoviedb.org/privacy-policy';
export const TERMS_OF_USE_LINK = 'https://www.themoviedb.org/api-terms-of-use';

const config = {
  API_KEY,
  ENDPOINT_URL_BASE,
  ENDPOINT_URL_IMAGES_w92,
  ENDPOINT_URL_IMAGES_w500,
  PRIVACY_POLICY_LINK,
  TERMS_OF_USE_LINK
};

export default config;
