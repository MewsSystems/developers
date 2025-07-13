import Link from 'next/link';
import { createMovieSlug } from '@/lib/slug';
import { formatPostImageAlt } from '@/lib/format';
import { MovieSearchResult } from '@/types/api';
import { MoviePoster } from '@/components/MoviePoster';
import { ReleaseDate, Score } from '@/components/DescriptionListItem';
import { Card } from '@/components/Card';

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

  return (
    <Card className={movieListItemContainerClasses}>
      <MoviePoster posterUrl={movie.poster_url} alt={formatPostImageAlt(movie.title)} />
      <div className="flex-1 pt-1 sm:pt-0">
        <h3 id={slug}>
          <Link
            href={{
              pathname: `/movies/${slug}`,
              query: {
                search,
                ...(page ? { page } : {}),
              },
            }}
            className="text-lg font-bold text-cyan-800 hover:underline"
          >
            {movie.title}
          </Link>
        </h3>
        <div>
          {movie.original_title !== movie.title && (
            <p className="text-cyan-700 text-sm italic">{movie.original_title}</p>
          )}
          <dl>
            <ReleaseDate date={movie.release_date} isSmall />
            <Score score={movie.vote_average} count={movie.vote_count} isSmall />
          </dl>
          {movie.overview && (
            <p className="hidden mt-1 text-stone-700 text-sm sm:line-clamp-6 sm:display md:line-clamp-9">
              {movie.overview}
            </p>
          )}
        </div>
      </div>
    </Card>
  );
}
