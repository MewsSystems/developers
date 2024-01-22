import axios from "axios"

const API_KEY = import.meta.env.VITE_API_KEY
const BASE_URL = "https://api.themoviedb.org/3"
export const IMAGE_BASE_URL = "https://image.tmdb.org/t/p/w1280"

export const moviesApi = axios.create({
  baseURL: BASE_URL,
  params: { api_key: API_KEY },
})

export const getMovieSearch = (query: string, page = 0, limit = 10) =>
  moviesApi.get("/search/movie", {
    params: { limit, offset: page * 10, query },
  })

export const getMovieDetails = (movieId: string | number) =>
  moviesApi.get(`/movie/${movieId}`)