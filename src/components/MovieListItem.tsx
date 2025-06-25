import { AiOutlineFileImage } from 'react-icons/ai';
import Image from 'next/image';
import Link from 'next/link';
import { TMDBMovie } from '@/types/tmdb';
import { createMovieSlug } from '@/lib/slug';

interface Props {
  movie: TMDBMovie & {
    poster_url: { default: string | null };
  };
}

export function MovieListItem({ movie }: Props) {
  const slug = createMovieSlug(movie.id, movie.original_title);

  return (
    <div className="flex gap-4 bg-white rounded-lg">
      <div className="w-[154px] h-[231px] bg-stone-100 flex items-center justify-center">
        {movie.poster_url.default ? (
          <Image
            src={movie.poster_url.default}
            alt={movie.title}
            width={154}
            height={231}
            className="object-cover"
          />
        ) : (
          <AiOutlineFileImage className="text-stone-400 text-4xl" />
        )}
      </div>
      <div className="flex-1">
        <h3>
          <Link
            href={`/movies/${slug}`}
            className="text-lg font-semibold text-blue-700 hover:underline"
          >
            {movie.title}
          </Link>
        </h3>
        {movie.original_title !== movie.title && (
          <p className="text-stone-400 text-sm">{movie.original_title}</p>
        )}
        <p className="text-stone-500 text-sm mt-1">
          {new Date(movie.release_date).toLocaleDateString('en-GB')} –{' '}
          {Math.round(movie.vote_average * 10)}%
        </p>
        <p className="mt-2 text-stone-700 text-sm">{movie.overview}</p>
      </div>
    </div>
  );
}
