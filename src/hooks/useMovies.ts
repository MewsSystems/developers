// import {
//   userListSchema,
//   type StateSchema,
//   type UserListSchema,
// } from '@/schemas/usersSchema'
import { useQuery } from '@tanstack/react-query'

import { api } from '../api/index'
import { movieKeys } from '../api/queryKeys'

const getAllMovies = async () => {
  const response = await api.get('movie/11')
  const data = await response.json()

  return data
}

export const useMovies = () => {
  return useQuery({
    queryKey: movieKeys.all,
    queryFn: getAllMovies,
  })
}
