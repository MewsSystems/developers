import { AiOutlineFileImage } from 'react-icons/ai';
import Image from 'next/image';
import Link from 'next/link';
import { TMDBMovie } from '@/types/tmdb';
import { createMovieSlug } from '@/lib/slug';
import { formatDate, formatPostImageAlt, formatVoteFromSearch } from '@/lib/format';

interface Props {
  movie: TMDBMovie & {
    poster_url: { default: string | null };
  };
  search: string;
  page?: number;
}

export function MovieListItem({ movie, search, page }: Props) {
  const slug = createMovieSlug(movie.id, movie.original_title);

  return (
    <div className="flex gap-4 bg-white rounded-xl p-3 h-[255px] border border-cyan-200">
      <div
        className={`w-[154px] flex justify-center ${movie.poster_url.default ? 'items-start' : 'bg-stone-100 items-center h-[231px] rounded-md'}`}
      >
        {movie.poster_url.default ? (
          <Image
            src={movie.poster_url.default}
            alt={formatPostImageAlt(movie.title)}
            width={154}
            height={231}
            className="rounded-md w-[154px] h-auto max-h-[231px] object-contain"
          />
        ) : (
          <AiOutlineFileImage className="text-stone-400 text-4xl" />
        )}
      </div>
      <div className="flex-1">
        <h3>
          <Link
            href={{
              pathname: `/movies/${slug}`,
              query: {
                search,
                ...(page ? { page } : {}),
              },
            }}
            className="text-lg font-semibold text-purple-800 hover:underline"
          >
            {movie.title}
          </Link>
        </h3>
        {movie.original_title !== movie.title && (
          <p className="text-stone-400 text-sm">{movie.original_title}</p>
        )}
        <p className="text-stone-500 text-sm mt-1">Date: {formatDate(movie.release_date)}</p>
        <p className="text-stone-500 text-sm mt-1">
          Score: {formatVoteFromSearch(movie.vote_average)}
        </p>
        {movie.overview && (
          <p className="hidden mt-2 text-stone-700 text-sm sm:line-clamp-4 sm:display md:line-clamp-6">
            {movie.overview}
          </p>
        )}
      </div>
    </div>
  );
}
