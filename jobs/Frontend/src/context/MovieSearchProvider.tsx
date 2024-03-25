import React, { createContext, useContext, useState } from 'react'

interface PaginationContext {
  currentPage: number
  totalPages: number
  query: string
  setCurrentPage: React.Dispatch<React.SetStateAction<number>>
  setTotalPages: React.Dispatch<React.SetStateAction<number>>
  setQuery: React.Dispatch<React.SetStateAction<string>>
}

const PaginationContext = createContext<PaginationContext>({
  currentPage: 1,
  totalPages: 1,
  query: '',
  setCurrentPage: () => {},
  setTotalPages: () => {},
  setQuery: () => {},
})

export const MovieSearchProvider = ({ children }) => {
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [query, setQuery] = useState('')

  return (
    <PaginationContext.Provider
      value={{
        currentPage,
        totalPages,
        query,
        setCurrentPage,
        setTotalPages,
        setQuery,
      }}
    >
      {children}
    </PaginationContext.Provider>
  )
}

export const useMovieSearch = () => useContext(PaginationContext)
