'use client'

import { setPage, setQuery } from '@/lib/features/filter/filterSlice'
import { useDispatch } from '@/lib/hooks'
import { ChangeEvent } from 'react'
import { Wrapper } from './Filterbar.styles'
import { Searchbar } from '@/components'

export const Filterbar = () => {
  const dispatch = useDispatch()

  const handleLoad = (query: string) => {
    dispatch(setPage('1'))
    dispatch(setQuery(query))
  }

  const handleSearch = (event: ChangeEvent<HTMLInputElement>) => {
    dispatch(setQuery(event.target.value))
    // prevent use from e.g. searching for 10th page for query where only two pages exist
    dispatch(setPage('1'))
  }

  return (
    <Wrapper>
      <Searchbar
        onLoad={handleLoad}
        onChange={handleSearch}
        placeholder="Search for a movie..."
      />
    </Wrapper>
  )
}
