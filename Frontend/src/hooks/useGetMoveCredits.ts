import axios, { CancelToken } from 'axios'
import { useCallback, useState, useEffect } from 'react'
import { Credits } from 'model/api/Credits'
import { CreditsApi } from 'api/Credits'

export const useGetMovieCredits = (id: number) => {
  const [credits, setCredits] = useState<Credits | undefined>()
  const [isLoading, setIsLoading] = useState(false)
  const [hasErrored, setHasErrored] = useState(false)

  const getMovieCredits = useCallback(
    async (cancelToken: CancelToken) => {
      try {
        setIsLoading(true)
        const { data } = await CreditsApi.getMovieCredits(id, {
          cancelToken,
        })

        setCredits(data)
        setIsLoading(false)
      } catch (e) {
        setHasErrored(true)
      }
    },
    [id]
  )

  useEffect(() => {
    const cancelToken = axios.CancelToken.source()
    getMovieCredits(cancelToken.token)

    return () => cancelToken.cancel()
  }, [getMovieCredits])

  return { credits, isLoading, hasErrored }
}
