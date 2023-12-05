import { useGetMoviesQuery } from "@/features/api/apiSlice"
import MovieCard, { MovieInterface } from "@/components/MovieCard"

function SearchPage() {
  const {
    data: movies,
    isLoading,
    isSuccess,
    isError,
    error,
  } = useGetMoviesQuery({ term: "harry", page: 1 })

  return (
    <div>
      <input placeholder="Search a movie..." />
      <section>
        {isLoading && <p>Loading...</p>}
        {isSuccess && (
          <>
            <p>Total results: {movies.total_results}</p>
            <p>
              Page {movies.page} of {movies.total_pages}
            </p>
            {movies.results.map((movie: MovieInterface) => (
              <MovieCard key={movie.id} movie={movie} />
            ))}
          </>
        )}
        {isError && <p>An error ocurred: {error.toString()}</p>}
      </section>
    </div>
  )
}

export default SearchPage
