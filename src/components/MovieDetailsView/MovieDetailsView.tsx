import { MovieDetailResponse } from '@/types/api';
import { ChipList } from '@/components/ChipList';
import { formatPostImageAlt, formatRuntime } from '@/lib/format';
import { LargeMoviePoster } from '../MoviePoster';
import { Card } from '@/components/Card';
import { DescriptionListItem, ReleaseDate, Score } from '@/components/DescriptionListItem';
import { OverviewContent } from '@/components/MovieDetailsView/OverviewContent';

interface Props {
  movie: MovieDetailResponse;
}

export function MovieDetailsView({ movie }: Props) {
  const hasPosterImage = movie.poster_url.default || movie.poster_url.sm || movie.poster_url.lg;

  return (
    <Card className="flex-col gap-4 p-3 sm:p-4 mb-8">
      <div className="grid gap-4 grid-cols-1 sm:grid-cols-[342px_1fr] lg:grid-cols-[500px_1fr]">
        <div className="flex flex-col items-start col-span-1 sm:col-span-2">
          <h1 className="text-2xl font-extrabold mb-0">{movie.title}</h1>
          {movie.original_title !== movie.title && (
            <h2 className="text-lg font-bold italic text-cyan-700 italic mb-0">
              AKA: {movie.original_title}
            </h2>
          )}
        </div>

        {hasPosterImage && (
          <div className="flex justify-center">
            <LargeMoviePoster posterUrl={movie.poster_url} alt={formatPostImageAlt(movie.title)} />
          </div>
        )}

        <div
          className={
            hasPosterImage ? `sm:col-span-1 sm:col-start-2 sm:row-start-2 sm:row-end-3` : undefined
          }
        >
          {movie.tagline && (
            <div>
              <h3 className="text-md font-bold text-stone-900">Tagline:</h3>
              <p className="text-cyan-700 italic block mb-0">{movie.tagline}</p>
            </div>
          )}

          <dl className="mb-2 mt-4">
            <ReleaseDate date={movie.release_date} />
            <Score score={movie.vote_average} count={movie.vote_count} />
            {movie.status && (
              <DescriptionListItem
                term="Status: "
                detail={movie.status}
                detailClassName="inline"
                termClassName="before:mt-2"
              />
            )}
            <DescriptionListItem
              term="Runtime: "
              detail={formatRuntime(movie.runtime)}
              detailClassName="inline"
              termClassName="before:mt-2"
            />
            {!!movie.spoken_languages.length && (
              <DescriptionListItem
                term="Languages:"
                detailClassName="mb-1 mt-1"
                termClassName="before:mt-2"
                detail={
                  <ChipList
                    items={movie.spoken_languages.map((language) => language.english_name)}
                    bgColor="bg-cyan-700"
                    textColor="text-white"
                  />
                }
              />
            )}
            {!!movie.origin_country.length && (
              <DescriptionListItem
                term="Origin Countries:"
                detailClassName="mb-1 mt-1"
                termClassName="before:mt-2"
                detail={
                  <ChipList
                    items={movie.origin_country}
                    bgColor="bg-cyan-700"
                    textColor="text-white"
                  />
                }
              />
            )}
          </dl>

          <div
            className="hidden lg:grid lg:mt-6 lg:gap-4"
            data-testid="integrated-overview-content"
          >
            <OverviewContent movie={movie} />
          </div>
        </div>

        <div
          className="grid grid-cols-1 gap-4 sm:col-start-1 sm:col-span-2 lg:hidden"
          data-testid="standalone-overview-content"
        >
          <OverviewContent movie={movie} />
        </div>
      </div>
    </Card>
  );
}
