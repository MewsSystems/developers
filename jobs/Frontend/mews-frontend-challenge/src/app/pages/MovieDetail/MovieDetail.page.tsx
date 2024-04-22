import { Poster } from "@/app/components/Poster";
import { BaseComponentProps } from "@/types";
import { cn, isEmpty } from "@/design-system/lib/utils";
import { AudienceScore } from "./components/AudienceScore";
import { Cast } from "./components/Cast";
import { MovieTitle } from "./components/MovieTitle";
import { CountryGenreAndRuntime } from "./components/CountryGenreAndRuntime";
import { Crew } from "./components/Crew";
import { getDirectors, useMovieData } from "./MovieDetail.data";
import { getTMDBMovieLink } from "@/app/services/tmdb";
import { useParams } from "react-router-dom";

export function MovieDetail() {
  const { id = "" } = useParams<{ id: string }>();

  const { cast, crew, movie } = useMovieData(id);
  const directors = getDirectors(crew);

  if (!movie) {
    return null;
  }

  return (
    <article className="mx-auto flex w-full max-w-7xl flex-col gap-2 p-4 sm:gap-8 sm:p-12 md:flex-row">
      <Poster
        className="h-auto w-full max-w-[360px] flex-none rounded-md object-cover sm:h-[540px] sm:w-[360px]"
        poster={movie.poster_path}
        alt={movie.title || "Movie poster"}
      />
      <section className="flex flex-col gap-8 py-6">
        <section aria-label="Movie title, genre and runtime">
          <MovieTitle title={movie.title!} releaseDate={movie.release_date} />
          <CountryGenreAndRuntime
            country={movie.origin_country}
            genres={movie.genres}
            runtime={movie.runtime!}
          />
        </section>

        {!!movie.vote_average && (
          <MovieDetailSection title="Audience score">
            <AudienceScore
              voteAverage={movie.vote_average}
              voteCount={movie.vote_count!}
            />
          </MovieDetailSection>
        )}

        <MovieDetailSection
          title={movie.tagline || "Overview"}
          aria-label="Overview"
        >
          <p>{movie.overview}</p>
        </MovieDetailSection>

        {!isEmpty(directors) && (
          <MovieDetailSection title="Directed by">
            <Crew crew={directors} />
          </MovieDetailSection>
        )}

        {!isEmpty(cast) && (
          <MovieDetailSection title="Stars">
            <Cast cast={cast} />
          </MovieDetailSection>
        )}

        <a
          className="font-bold after:content-['_↗']"
          aria-label="See more information about the movie"
          target="_blank"
          rel="noopener noreferrer"
          href={getTMDBMovieLink(id)}
        >
          More information
        </a>
      </section>
    </article>
  );
}

function MovieDetailSection({
  title,
  children,
  className,
  "aria-label": _ariaLabel = title,
  ...props
}: { title: string } & BaseComponentProps) {
  return (
    <section
      className={cn(className, "flex flex-col gap-2")}
      aria-label={_ariaLabel}
      {...props}
    >
      <h3 className="text-ellipsis text-sm font-bold uppercase">{title}</h3>
      {children}
    </section>
  );
}
