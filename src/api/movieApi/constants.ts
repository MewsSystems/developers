export const MOVIE_API_BASE_URL = 'https://api.themoviedb.org/3';
export const IMAGE_BASE_URL = 'https://image.tmdb.org/t/p/w500';

export const API_STATUS_MESSAGE = {
  INVALID_API_KEY: 'Invalid API key: You must be granted a valid key.',
  AUTHENTICATION_FAILED:
    'Authentication failed: You do not have permissions to access the service.',
  INVALID_TOKEN: 'Invalid token.',
  RESOURCE_NOT_FOUND: 'The resource you requested could not be found.',
  INVALID_ID: 'The ID is invalid.',
  INTERNAL_ERROR: 'Internal error: Something went wrong, contact TMDB.',
  FAILED: 'Failed',
} as const;

export const ERRORS_BY_HTTP_STATUS = {
  401: {
    [API_STATUS_MESSAGE.INVALID_API_KEY]:
      'Your API key is invalid. Please check your configuration.',
    [API_STATUS_MESSAGE.AUTHENTICATION_FAILED]: "You don't have permission to access this service.",
    [API_STATUS_MESSAGE.INVALID_TOKEN]: 'Your session has expired. Please try again.',
  },
  404: {
    [API_STATUS_MESSAGE.RESOURCE_NOT_FOUND]: "We couldn't find what you're looking for.",
  },
  500: {
    [API_STATUS_MESSAGE.INVALID_ID]: 'Invalid movie ID provided.',
    [API_STATUS_MESSAGE.INTERNAL_ERROR]: 'Something went wrong with the movie database.',
    [API_STATUS_MESSAGE.FAILED]: 'Something went wrong. Please try again later.',
  },
} as const;
