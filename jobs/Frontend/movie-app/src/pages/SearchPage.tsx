import { useSelector } from "react-redux"
import MovieCard, { MovieInterface } from "@/components/MovieCard"

function SearchPage() {
  const movies = useSelector((state) => state.movies)
  return (
    <div>
      <input placeholder="Search a movie..." />
      <section>
        <p>Total results: {movies.total_results}</p>
        {movies.results.map((movie: MovieInterface) => (
          <MovieCard movie={movie} />
        ))}
      </section>
    </div>
  )
}

export default SearchPage
