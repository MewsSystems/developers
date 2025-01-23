import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { FixedSizeList as List } from "react-window"
import { useInfiniteQuery } from "@tanstack/react-query"
import { fetchMovies } from "@api/movies"
import useDebounce from "@hooks/useDebounce"
import Input from "@components/Input"
import Loading from "@components/Loading"
import { Movie, MovieResponse } from "@types"
import Row from "./components/Row"
import ViewModal from "./components/ViewModal"

export const MovieListPage = () => {
  const [query, setQuery] = useState("")
  const [selectedMovie, setSelectedMovie] = useState<Movie | null>(null)
  const debouncedQuery = useDebounce(query, 500)
  const navigate = useNavigate()

  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, status } = useInfiniteQuery({
    queryKey: ["movies", debouncedQuery],
    queryFn: (context) => fetchMovies(debouncedQuery, context),
    initialPageParam: 1,
    getNextPageParam: (lastPage: MovieResponse) => {
      return lastPage.page < lastPage.total_pages ? lastPage.page + 1 : undefined
    },
    enabled: !!debouncedQuery,
  })

  useEffect(() => {
    const urlParams = new URLSearchParams(location.search)
    setQuery(urlParams.get("query") ?? "")
  }, [])

  useEffect(() => {
    const searchParams = new URLSearchParams()
    searchParams.set("query", query)
    navigate(`?${searchParams.toString()}`, { replace: true })
  }, [query])

  const movies = data?.pages.flatMap((page) => page.results) || []
  const count = movies.length
  const totalCount = data?.pages?.[0].total_results ?? 0

  const loadMoreRows = async () => {
    if (hasNextPage && !isFetchingNextPage) {
      fetchNextPage()
    }
  }

  const renderWindow = () => {
    if (!debouncedQuery) {
      return (
        <p className="p-4 text-center text-xl text-gray-600">
          Please type a keyword to search movies ...
        </p>
      )
    }

    if (status === "pending") {
      return (
        <div className="flex w-full justify-center">
          <Loading />
        </div>
      )
    }

    if (status === "error") {
      return (
        <p className="p-4 text-center text-xl text-red-600">
          Error occurred. Try again after refreshing the page.
        </p>
      )
    }

    return (
      <List
        itemCount={hasNextPage ? movies.length + 1 : movies.length}
        itemSize={120}
        height={window.innerHeight - 144}
        onItemsRendered={({ visibleStopIndex }: { visibleStopIndex: number }) => {
          if (visibleStopIndex >= movies.length - 1) {
            loadMoreRows()
          }
        }}
      >
        {({ index, style }: { index: number; style: React.CSSProperties }) => (
          <Row
            movie={movies[index]}
            style={style}
            onClick={() => setSelectedMovie(movies[index])}
          />
        )}
      </List>
    )
  }

  return (
    <div className="h-full">
      <Input
        value={query}
        setValue={setQuery}
        placeholder="Search movies by their original, translated and alternative titles..."
      />
      {renderWindow()}
      {movies.length ? (
        <p className="py-2 text-right">
          Showing {count} of {totalCount} movies
        </p>
      ) : null}
      <ViewModal
        movie={selectedMovie}
        isOpen={!!selectedMovie}
        onDismiss={() => setSelectedMovie(null)}
      />
    </div>
  )
}

export default MovieListPage
