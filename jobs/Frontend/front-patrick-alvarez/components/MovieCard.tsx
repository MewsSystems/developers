import { Movie } from '@/types/Movie'

interface MovieCardProps {
    movie: Movie
}

const MovieCard = ({ movie }: MovieCardProps) => {
    const year = new Date(movie.release_date).getFullYear()
    const baseImageUrl = 'https://image.tmdb.org/t/p/w500'

    return (
        <div
            className="flex flex-col relative w-full h-48 rounded-lg bg-cover bg-center items-start justify-end p-4"
            style={{
                backgroundImage: movie.backdrop_path
                    ? `url(${baseImageUrl}${movie.backdrop_path})`
                    : 'none',
            }}
        >
            <div className="bg-black/50 rounded-lg p-2">
                <p className="text-white text-sm">{movie.title}</p>
                <p className="text-white/80 text-xs">{year}</p>
            </div>
        </div>
    )
}

export default MovieCard
