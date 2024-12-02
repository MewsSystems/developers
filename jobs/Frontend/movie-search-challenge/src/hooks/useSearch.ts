import { useRef } from "react"
import { useAppDispatch, useAppSelector } from "../app/hooks"
import { setSearchQuery } from "../app/slices/movieList/movieListSlice"
import { getMovies } from "../app/slices/movieList/thunks"
import debounce from "lodash.debounce"

const useSearch = () => {
  const dispatch = useAppDispatch()
  const { searchQuery } = useAppSelector(state => state.movieList)

  const delayedSearch = debounce(() => {
    dispatch(getMovies())
  }, 500)

  const delayedSearchReference = useRef(delayedSearch).current

  const setSearchValue = (query: string) => {
    dispatch(setSearchQuery({ searchQuery: query }))
    delayedSearchReference()
  }

  return [searchQuery, setSearchValue] as const
}

export default useSearch
