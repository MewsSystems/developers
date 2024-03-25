import { API_KEY, API_URL } from '@/const'
import { APIResponse, Movie } from '@/types'

const searchMovies = async (
    query: string,
    page: number
): Promise<APIResponse> => {
  const data = await fetch(
      `${API_URL}/search/movie?query=${query}&api_key=${API_KEY}&page=${page}`
  )
  return data.json()
}

const getMovieDetails = async (id: number): Promise<Movie> => {
  const data = await fetch(`${API_URL}/movie/${id}?api_key=${API_KEY}`)
  return data.json()
}

export const movieService = {
  searchMovies,
  getMovieDetails
}


