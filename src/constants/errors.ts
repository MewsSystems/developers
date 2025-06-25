export const ERROR_CODES = {
  AUTH_ERROR: "AUTH_ERROR",
  FORBIDDEN: "FORBIDDEN",
  NOT_FOUND: "NOT_FOUND",
  RATE_LIMIT: "RATE_LIMIT",
  SERVER_ERROR: "SERVER_ERROR",
  NETWORK_ERROR: "NETWORK_ERROR",
  UNKNOWN_ERROR: "UNKNOWN_ERROR",
} as const

export const ERROR_MESSAGES = {
  AUTH_FAILED: "Authentication failed. Please check your API key.",
  ACCESS_FORBIDDEN: "Access forbidden. Insufficient permissions.",
  RESOURCE_NOT_FOUND: "Resource not found",
  RATE_LIMIT_EXCEEDED: "Too many requests. Please try again later.",
  SERVER_ERROR: "Server error. Please try again later.",
  NETWORK_ERROR: "Network error. Please check your connection.",
  UNEXPECTED_ERROR: "An unexpected error occurred",
  POPULAR_MOVIES_NOT_FOUND: "No popular movies found for this page",
  POPULAR_MOVIES_FETCH_FAILED: "Failed to fetch popular movies",
  SEARCH_QUERY_EMPTY: "Search query cannot be empty",
  MOVIE_SEARCH_FAILED: (query: string) => `Failed to search for movies with query: "${query}"`,
  INVALID_MOVIE_ID: "Invalid movie ID provided",
  MOVIE_NOT_FOUND: (id: number) => `Movie with ID ${id} not found`,
  MOVIE_FETCH_FAILED: (id: number) => `Failed to fetch movie with ID: ${id}`,
} as const

export type ApiErrorCode = keyof typeof ERROR_CODES
