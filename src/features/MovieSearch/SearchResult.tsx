import { Movie } from '@/schemas/movie'
import { MovieCard } from './MovieCard'

type Props = {
  movies: Movie[] | null
}

export const SearchResult = ({ movies }: Props) => {
  if (!movies || movies.length === 0) {
    return (
      <div className="text-center text-2xl font-bold flex justify-center items-center mt-8">
        No results found
      </div>
    )
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </div>
  )
}
