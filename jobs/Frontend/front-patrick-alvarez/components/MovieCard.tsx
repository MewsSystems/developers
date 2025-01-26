import { API_IMAGE_BASE_URL } from '@/const/endpoints'
import Movie from '@/types/Movie'
import Link from 'next/link'

interface MovieCardProps {
    movie: Movie
}

const MovieCard = ({ movie }: MovieCardProps) => {
    const year = movie.releaseDate
        ? new Date(movie.releaseDate).getFullYear()
        : null

    return (
        <Link href={`/${movie.id}`}>
            <div
                className="relative flex h-48 w-full flex-col items-start justify-end rounded-lg bg-cover bg-center p-4"
                style={{
                    backgroundImage: movie.backdropPath
                        ? `url(${API_IMAGE_BASE_URL}${movie.backdropPath})`
                        : 'none',
                    backgroundColor: !movie.backdropPath
                        ? '#1f2937'
                        : undefined,
                }}
            >
                <div className="rounded-lg bg-black/50 p-2">
                    <p className="text-sm text-white">
                        {movie.title || 'Untitled'}
                    </p>
                    {year && <p className="text-xs text-white/80">{year}</p>}
                </div>
            </div>
        </Link>
    )
}

export default MovieCard
