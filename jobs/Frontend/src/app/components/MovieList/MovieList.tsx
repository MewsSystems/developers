'use client'

import { useDispatch, useSelector } from '@/lib/hooks'
import { NoMatch } from '../NoMatch'
import { useGetMoviesQuery } from '@/lib/features/api/apiSlice'
import { setPage } from '@/lib/features/filter/filterSlice'
import { BaseMovieList } from './BaseMovieList'
import { Movie } from '../Movie'

export const MovieList = () => {
  const dispatch = useDispatch()
  const currentPage = useSelector((state) => state.filter.page)
  const searchQuery = useSelector((state) => state.filter.query)

  const { data, isFetching } = useGetMoviesQuery({
    page: currentPage,
    query: searchQuery,
  })

  if (isFetching || !data) {
    return
  }

  if (!isFetching && data.total_results === 0) {
    return <NoMatch />
  }

  const handleSetPage = (page: number) => {
    dispatch(setPage(page.toString()))
  }

  return (
    <BaseMovieList onSetPage={handleSetPage} totalPages={data.total_pages}>
      {data.results.map(({ id, ...props }) => (
        <Movie key={id} id={id} {...props} />
      ))}
    </BaseMovieList>
  )
}
