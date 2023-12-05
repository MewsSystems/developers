import { useSelector } from "react-redux"

function SearchPage() {
  const movies = useSelector((state) => state.movies)
  return (
    <div>
      <input placeholder="Search a movie..." />
      <section>
        {movies.results.map((movie) => (
          <div>{movie.title}</div>
        ))}
      </section>
    </div>
  )
}

export default SearchPage
