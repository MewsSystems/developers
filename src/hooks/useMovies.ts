import { useQuery } from '@tanstack/react-query'

import { api } from '@/api/index'
import { movieKeys } from '@/api/queryKeys'
import {
  type MovieSearchResult,
  MovieSearchResultSchema,
} from '@/schemas/movie'

const getSearchMovieResult = async (search: string, page: number) => {
  const response = await api.get(`search/movie?query=${search}&page=${page}`)
  const data = await response.json()

  return MovieSearchResultSchema.parse(data)
}

export const useMovies = (search: string, page: number) => {
  return useQuery<MovieSearchResult>({
    queryKey: [movieKeys.searchResult, search, page],
    queryFn: () => getSearchMovieResult(search, page),
  })
}
