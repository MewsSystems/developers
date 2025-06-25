import axios from "axios"
import { type ApiErrorCode, ERROR_CODES, ERROR_MESSAGES } from "../constants/errors"

const BASE_URL = import.meta.env.VITE_TMDB_BASE_URL || "https://api.themoviedb.org/3"
const API_KEY = import.meta.env.VITE_TMDB_API_KEY || "mock-api-key"

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
          throw new ApiError(ERROR_MESSAGES.AUTH_FAILED, status, ERROR_CODES.AUTH_ERROR)

        case 403:
          throw new ApiError(ERROR_MESSAGES.ACCESS_FORBIDDEN, status, ERROR_CODES.FORBIDDEN)

        case 404:
          throw new ApiError(
            data?.status_message || ERROR_MESSAGES.RESOURCE_NOT_FOUND,
            status,
            ERROR_CODES.NOT_FOUND
          )

        case 429:
          throw new ApiError(ERROR_MESSAGES.RATE_LIMIT_EXCEEDED, status, ERROR_CODES.RATE_LIMIT)

        case 500:
        case 502:
        case 503:
        case 504:
          throw new ApiError(ERROR_MESSAGES.SERVER_ERROR, status, ERROR_CODES.SERVER_ERROR)

        default:
          throw new ApiError(
            data?.status_message || ERROR_MESSAGES.UNEXPECTED_ERROR,
            status,
            ERROR_CODES.UNKNOWN_ERROR
          )
      }
    } else if (error.request) {
      throw new ApiError(ERROR_MESSAGES.NETWORK_ERROR, 0, ERROR_CODES.NETWORK_ERROR)
    } else {
      throw new ApiError(ERROR_MESSAGES.UNEXPECTED_ERROR, 0, ERROR_CODES.UNKNOWN_ERROR)
    }
  }
)
