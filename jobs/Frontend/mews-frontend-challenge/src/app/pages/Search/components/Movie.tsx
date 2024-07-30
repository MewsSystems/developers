import { MovieResult } from "@/app/services/tmdb";
import { Link } from "react-router-dom";

import { getMovieDetailRoute } from "@/app/AppRouter.utils";
import { BaseComponentProps } from "@/types";
import { Poster } from "@/app/components/Poster";

export type MovieProps = BaseComponentProps & {
  movie: MovieResult;
};

export function Movie({ movie, ...props }: MovieProps) {
  const hasOriginalTitle = movie.title !== movie.original_title;
  const releaseDate = movie.release_date
    ? new Date(movie.release_date)
    : undefined;

  return (
    <article
      className="flex gap-4 rounded-lg border-b-[1px] p-2 transition-shadow hover:shadow-md sm:gap-8 sm:p-4"
      {...props}
    >
      <section className="w-24 flex-none">
        <Link to={getMovieDetailRoute(movie.id)}>
          <Poster
            className="h-[144px] w-[96px] rounded-md ring-primary hover:ring-2"
            poster={movie.poster_path}
            resolution="w300_and_h450"
            alt={movie.title || "Movie poster"}
          />
        </Link>
      </section>
      <section className="flex flex-col">
        <h2 className="text-xl font-bold">
          <Link
            className="text-primary underline-offset-4 hover:underline"
            to={getMovieDetailRoute(movie.id)}
          >
            {movie.title}
          </Link>{" "}
          {hasOriginalTitle && (
            <span className="text-secondary">({movie.original_title})</span>
          )}
        </h2>
        {releaseDate && (
          <span className="text-secondary">{releaseDate.getFullYear()}</span>
        )}
        <p className="mt-4 line-clamp-3">{movie.overview}</p>
      </section>
    </article>
  );
}
