import axios from "axios";

export const apiInstance = axios.create({
  baseURL: import.meta.env.VITE_MOVIE_DB_BASE_URL,
  timeout: 5000,
  headers: {
    "Content-Type": "application/json",
  },
  params: {
    api_key: import.meta.env.VITE_MOVIE_DB_API_KEY,
  },
});
