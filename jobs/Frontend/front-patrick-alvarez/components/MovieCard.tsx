import { Movie } from '@/types/Movie';
import Link from 'next/link';

interface MovieCardProps {
    movie: Movie
}

const MovieCard = ({ movie }: MovieCardProps) => {
    const year = movie.release_date 
        ? new Date(movie.release_date).getFullYear()
        : null;
    const baseImageUrl = 'https://image.tmdb.org/t/p/w500'

    return (
        <Link href={`/${movie.id}`}>
            <div
                className="relative flex h-48 w-full flex-col items-start justify-end rounded-lg bg-cover bg-center p-4"
                style={{
                    backgroundImage: movie.backdrop_path
                        ? `url(${baseImageUrl}${movie.backdrop_path})`
                        : 'none',
                    backgroundColor: !movie.backdrop_path ? '#1f2937' : undefined,
                }}
            >
                <div className="rounded-lg bg-black/50 p-2">
                    <p className="text-sm text-white">{movie.title || 'Untitled'}</p>
                    {year && <p className="text-xs text-white/80">{year}</p>}
                </div>
            </div>
        </Link>
    )
}

export default MovieCard
