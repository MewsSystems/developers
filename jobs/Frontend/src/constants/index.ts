export enum ActionKind {
  UPDATE_SEARCH_TERM = 'UPDATE_SEARCH_TERM',
  SEARCH_MOVIES_REQUEST = 'SEARCH_MOVIES_REQUEST',
  SEARCH_MOVIES_SUCCESS = 'SEARCH_MOVIES_SUCCESS',
  HYDRATE = 'HYDRATE',
};

export const IMAGE_THUMBNAIL_SOURCE = 'https://image.tmdb.org/t/p/w92';
export const IMAGE_DETAIL_SOURCE = 'https://image.tmdb.org/t/p/w185';

export const MINIMUM_SEARCH_TERM_LENGTH = 3;

export const CLIENT_API_URL = '/api/search';
export const SERVER_API_URL = 'https://api.themoviedb.org/3/search/movie';