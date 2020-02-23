import { MoviesApi } from 'api/Movies'
import axios, { CancelToken } from 'axios'
import { useCallback, useState, useEffect } from 'react'
import { MovieDetail } from 'model/api/MovieDetail'

export const useGetMovieDetail = (id: number) => {
  const [movieDetail, setMovieDetail] = useState<MovieDetail | undefined>()
  const [isLoading, setIsLoading] = useState(false)
  const [hasErrored, setHasErrored] = useState(false)

  const getMovieDetail = useCallback(
    async (cancelToken: CancelToken) => {
      try {
        setIsLoading(true)
        const { data } = await MoviesApi.getMovieDetail(id, undefined, {
          cancelToken,
        })

        setMovieDetail(data)
        setIsLoading(false)
      } catch (e) {
        setHasErrored(true)
      }
    },
    [id]
  )

  useEffect(() => {
    const cancelToken = axios.CancelToken.source()
    getMovieDetail(cancelToken.token)

    return () => cancelToken.cancel()
  }, [getMovieDetail])

  return { movieDetail, isLoading, hasErrored }
}
