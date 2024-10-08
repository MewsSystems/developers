"use client"
import { use } from "react"

import { MovieCardSearch, MoviesContainer } from "@/app/features/movie"

import { SearchContext } from "../providers/MovieSearchProvider"
export default function MovieSearchPage() {
  const { search, setSearch } = use(SearchContext)

  return (
    <>
      <MovieCardSearch search={search} setSearch={setSearch} />
      <MoviesContainer />
    </>
  )
}
