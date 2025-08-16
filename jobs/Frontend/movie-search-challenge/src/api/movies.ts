import axios from "axios"

const API_KEY = import.meta.env.VITE_API_KEY
const BASE_URL = "https://api.themoviedb.org/3"


export const moviesApi = axios.create({
  baseURL: BASE_URL,
  params: { api_key: API_KEY },
})

export const getDicoverMovies = (page = 1) =>
  moviesApi.get("/discover/movie", {
    params: { page },
  })

export const getMovieSearch = (query: string, page = 1) =>
  moviesApi.get("/search/movie", {
    params: { page, query },
  })

export const getMovieDetails = (movieId: string | number) =>
  moviesApi.get(`/movie/${movieId}`)
