import { clsx, type ClassValue } from 'clsx'
import { twMerge } from 'tailwind-merge'

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export const formatDate = (dateString: string | null) => {
  if (!dateString) return 'Unknown'
  const date = new Date(dateString)
  return date.toLocaleDateString('cs', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  })
}

export const SEARCH_DEBOUNCE_DELAY = 500

export const IMAGE_BASE_URL = 'https://image.tmdb.org/t/p/'

const GENRES = {
  28: 'Action',
  12: 'Adventure',
  16: 'Animation',
  35: 'Comedy',
  80: 'Crime',
  99: 'Documentary',
  18: 'Drama',
  10751: 'Family',
  14: 'Fantasy',
  36: 'History',
  27: 'Horror',
  10402: 'Music',
  9648: 'Mystery',
  10749: 'Romance',
  878: 'Science Fiction',
  10770: 'TV Movie',
  53: 'Thriller',
  10752: 'War',
  37: 'Western',
} as const

export const getGenres = (genreIds: number[]): string => {
  return genreIds
    .slice(0, 3)
    .map((id) => GENRES[id as keyof typeof GENRES] || '')
    .filter(Boolean)
    .join(', ')
}

export const raiseError = (message: string): never => {
  throw new Error(message)
}

export const getRatingColor = (rating: number): string => {
  switch (true) {
    case rating === 0:
      return 'text-gray-500'
    case rating > 0 && rating < 5:
      return 'text-red-600'
    case rating < 6.5:
      return 'text-orange-600'
    case rating < 8:
      return 'text-yellow-600'
    default:
      return 'text-green-600'
  }
}
