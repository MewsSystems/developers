import { API_KEY, API_URL } from '@/const'
import { SearchMoviesAPIResponse, Movie } from '@/types'

const searchMovies = async (
  query: string,
  page: number
): Promise<SearchMoviesAPIResponse> => {
  const url = `${API_URL}/search/movie?query=${query}&api_key=${API_KEY}&page=${page}`
  try {
    const response = await fetch(url)

    if (!response.ok) {
      throw new Error(`API call failed with status: ${response.status}`)
    }

    return response.json()
  } catch (error) {
    console.error('Failed to search movies:', error)
    throw error
  }
}

const getMovieDetails = async (id: number): Promise<Movie> => {
  const url = `${API_URL}/movie/${id}?api_key=${API_KEY}`
  try {
    const response = await fetch(url)

    if (!response.ok) {
      throw new Error(`API call failed with status: ${response.status}`)
    }

    return response.json()
  } catch (error) {
    console.error('Failed to fetch movie details:', error)
    throw error
  }
}

export const movieService = {
  searchMovies,
  getMovieDetails,
}
