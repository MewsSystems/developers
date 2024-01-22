import axios from "axios"

const API_KEY = import.meta.env.VITE_API_KEY
const BASE_URL = "https://api.themoviedb.org/3"
export const IMAGE_BASE_URL = "https://image.tmdb.org/t/p/w500"

export const moviesApi = axios.create({
  baseURL: BASE_URL,
  params: { api_key: API_KEY },
})
