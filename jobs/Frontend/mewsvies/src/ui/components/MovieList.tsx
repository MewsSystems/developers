import { Movie } from '../../domain/interfaces/movie.interface.ts'
import { MovieItem } from './MovieItem.tsx'

interface MovieListProps {
    movies: Movie[]
}

export default function MovieList({ movies }: MovieListProps) {
    const listItems = movies.map((movie) => (
        <div key={movie.id}>
            <MovieItem movie={movie} />
            <hr />
        </div>
    ))

    return <>{listItems}</>
}
