import axios from "axios"

const BASE_URL = import.meta.env.VITE_TMDB_BASE_URL || "https://api.themoviedb.org/3"
const API_KEY = import.meta.env.VITE_TMDB_API_KEY || "mock-api-key"

type ApiErrorCode =
  | "AUTH_ERROR"
  | "FORBIDDEN"
  | "NOT_FOUND"
  | "RATE_LIMIT"
  | "SERVER_ERROR"
  | "NETWORK_ERROR"
  | "UNKNOWN_ERROR"

export class ApiError extends Error {
  status: number
  code?: ApiErrorCode

  constructor(message: string, status: number, code?: ApiErrorCode) {
    super(message)
    this.name = "ApiError"
    this.status = status
    this.code = code
  }
}

export const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    Authorization: `Bearer ${API_KEY}`,
    "Content-Type": "application/json",
  },
})

api.interceptors.request.use(
  (config) => {
    if (import.meta.env.DEV) {
      console.log("🚀 API Request:", config.method?.toUpperCase(), config.url)
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

api.interceptors.response.use(
  (response) => {
    if (import.meta.env.DEV) {
      console.log("✅ API Response:", response.status, response.config.url)
    }
    return response
  },
  (error) => {
    if (import.meta.env.DEV) {
      console.error("❌ API Error:", error.response?.status, error.config?.url)
    }

    if (error.response) {
      const { status, data } = error.response

      switch (status) {
        case 401:
          throw new ApiError(
            "Authentication failed. Please check your API key.",
            status,
            "AUTH_ERROR"
          )

        case 403:
          throw new ApiError("Access forbidden. Insufficient permissions.", status, "FORBIDDEN")

        case 404:
          throw new ApiError(data?.status_message || "Resource not found", status, "NOT_FOUND")

        case 429:
          throw new ApiError("Too many requests. Please try again later.", status, "RATE_LIMIT")

        case 500:
        case 502:
        case 503:
        case 504:
          throw new ApiError("Server error. Please try again later.", status, "SERVER_ERROR")

        default:
          throw new ApiError(
            data?.status_message || "An unexpected error occurred",
            status,
            "UNKNOWN_ERROR"
          )
      }
    } else if (error.request) {
      throw new ApiError("Network error. Please check your connection.", 0, "NETWORK_ERROR")
    } else {
      throw new ApiError("An unexpected error occurred", 0, "UNKNOWN_ERROR")
    }
  }
)
