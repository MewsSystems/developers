import axios, { CancelToken } from 'axios'
import { useCallback, useState, useEffect } from 'react'
import { SearchApi } from 'api/Search'
import { useSelector } from 'react-redux'
import { State } from 'state/rootReducer'
import { Movie } from 'model/api/Movie'
import { List } from 'model/api/List'

export const useGetSearchResults = (page: number) => {
  const searchValue = useSelector((state: State) => state.search.value)
  const [results, setResults] = useState<List<Movie>>({
    page: 0,
    results: [],
    total_pages: 0,
    total_results: 0,
  })
  const [isLoading, setIsLoading] = useState(false)
  const [hasErrored, setHasErrored] = useState(false)

  const getSearchResults = useCallback(
    async (cancelToken: CancelToken) => {
      try {
        setIsLoading(true)
        const { data } = await SearchApi.getSearchResults(
          searchValue,
          page,
          undefined,
          undefined,
          undefined,
          undefined,
          {
            cancelToken,
          }
        )

        setResults(data)
        setIsLoading(false)
      } catch (e) {
        setHasErrored(true)
      }
    },
    [page, searchValue]
  )

  useEffect(() => {
    const cancelToken = axios.CancelToken.source()

    if (searchValue.length > 0) {
      getSearchResults(cancelToken.token)
    }

    return () => cancelToken.cancel()
  }, [getSearchResults, searchValue])

  return { results, isLoading, hasErrored }
}
