import Link from 'next/link';
import { createMovieSlug } from '@/lib/slug';
import { formatDate, formatPostImageAlt, formatVoteFromSearch } from '@/lib/format';
import { useId } from 'react';
import { MovieSearchResult } from '@/types/api';
import { MoviePoster } from '@/components/MoviePoster';

interface Props {
  movie: MovieSearchResult & {
    poster_url: { default: string | null };
  };
  search: string;
  page?: number;
}

export function MovieListItem({ movie, search, page }: Props) {
  const slug = createMovieSlug(movie.id, movie.original_title);
  const descriptionId = useId();

  return (
    <div className="flex gap-4 bg-white rounded-xl p-3 min-h-[255px] border border-cyan-200">
      <MoviePoster posterUrl={movie.poster_url} alt={formatPostImageAlt(movie.title)} />

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
            aria-describedby={descriptionId}
          >
            {movie.title}
          </Link>
        </h3>
        <div id={descriptionId}>
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
    </div>
  );
}
