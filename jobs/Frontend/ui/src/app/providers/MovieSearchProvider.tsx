"use client"
import { createContext, Dispatch, SetStateAction, useState } from "react"

interface SearchContextType {
  search: string
  setSearch: Dispatch<SetStateAction<string>>
}

export const SearchContext = createContext<SearchContextType>({
  search: "",
  setSearch: () => {},
})

export const MovieSearchProvider = ({
  children,
}: {
  children: React.ReactNode
}) => {
  const [search, setSearch] = useState("")

  return (
    <SearchContext.Provider value={{ search, setSearch }}>
      {children}
    </SearchContext.Provider>
  )
}
