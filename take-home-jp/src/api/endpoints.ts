export const BASE_API_URL = 'https://api.themoviedb.org/3/';
export const IMAGE_URL = 'https://image.tmdb.org/t/p/original';
export const THUMBNAIL_URL = 'https://image.tmdb.org/t/p/w500';
export const DEFAULT_IMG_URL =
  'https://img.freepik.com/premium-vector/real-transparent-plastic-wrap-texture-vector-background_37787-1377.jpg?size=626&ext=jpg&ga=GA1.1.1700460183.1708560000&semt=ais';

export const API_ENDPOINTS = {
  findById: (external_id: string) => `movie/${external_id}?`,
  searchByMovie: 'search/movie?',
};
