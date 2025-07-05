import Link from 'next/link';
import { createMovieSlug } from '@/lib/slug';
import { formatPostImageAlt, formatVoteFromSearch } from '@/lib/format';
import { useId } from 'react';
import { MovieSearchResult } from '@/types/api';
import { MoviePoster } from '@/components/MoviePoster';
import { ReleaseDate } from '@/components/MovieListItem/ReleaseDate';
import { Score } from '@/components/MovieListItem/Score';
import { Card } from '@/components//Card';

interface Props {
  movie: MovieSearchResult & {
    poster_url: { default: string | null };
  };
  search: string;
  page?: number;
}

export const movieListItemContainerClasses =
  'gap-4 p-0 pr-4 min-h-[137px] sm:p-3 sm:min-h-[257px] md:min-h-[304px]';

export function MovieListItem({ movie, search, page }: Props) {
  const slug = createMovieSlug(movie.id, movie.original_title);
  const descriptionId = useId();

  return (
    <Card className={movieListItemContainerClasses}>
      <MoviePoster posterUrl={movie.poster_url} alt={formatPostImageAlt(movie.title)} />
      <div className="flex-1 pt-1 sm:pt-0">
        <h3>
          <Link
            href={{
              pathname: `/movies/${slug}`,
              query: {
                search,
                ...(page ? { page } : {}),
              },
            }}
            className="text-lg font-bold text-cyan-800 hover:underline"
            aria-describedby={descriptionId}
          >
            {movie.title}
          </Link>
        </h3>
        <div id={descriptionId}>
          {movie.original_title !== movie.title && (
            <p className="text-cyan-700 text-sm italic">{movie.original_title}</p>
          )}
          <ReleaseDate date={movie.release_date} />
          <Score score={formatVoteFromSearch(movie.vote_average, movie.vote_count)} />
          {movie.overview && (
            <p className="hidden mt-2 text-stone-700 text-sm sm:line-clamp-7 sm:display md:line-clamp-9">
              {movie.overview}
            </p>
          )}
        </div>
      </div>
    </Card>
  );
}
