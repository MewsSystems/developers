import { useCallback, useEffect, useState } from "react"
import { useSearchParams } from "react-router"

export interface UseSearchStateReturn {
  searchQuery: string
  searchPage: number
  popularPage: number
  setSearchQuery: (query: string) => void
  setSearchPage: (page: number) => void
  setPopularPage: (page: number) => void
  resetSearch: () => void
}

export const useSearchState = (): UseSearchStateReturn => {
  const [searchParams, setSearchParams] = useSearchParams()

  const [searchQuery, setSearchQueryState] = useState(searchParams.get("q") || "")
  const [searchPage, setSearchPageState] = useState(Number(searchParams.get("search_page")) || 1)
  const [popularPage, setPopularPageState] = useState(Number(searchParams.get("popular_page")) || 1)

  useEffect(() => {
    const params = new URLSearchParams()

    if (searchQuery.trim()) {
      params.set("q", searchQuery.trim())
      params.set("search_page", searchPage.toString())
    } else {
      params.set("popular_page", popularPage.toString())
    }

    setSearchParams(params, { replace: true })
  }, [searchQuery, searchPage, popularPage, setSearchParams])

  const setSearchQuery = useCallback(
    (query: string) => {
      setSearchQueryState(query)
      if (query.trim() !== searchQuery.trim()) {
        setSearchPageState(1)
      }
    },
    [searchQuery]
  )

  const setSearchPage = useCallback((page: number) => {
    setSearchPageState(page)
  }, [])

  const setPopularPage = useCallback((page: number) => {
    setPopularPageState(page)
  }, [])

  const resetSearch = useCallback(() => {
    setSearchQueryState("")
    setSearchPageState(1)
  }, [])

  return {
    searchQuery,
    searchPage,
    popularPage,
    setSearchQuery,
    setSearchPage,
    setPopularPage,
    resetSearch,
  }
}
