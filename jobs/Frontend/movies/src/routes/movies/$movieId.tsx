import { createFileRoute } from "@tanstack/react-router";
import { useSuspenseQuery } from "@tanstack/react-query";
import { findMovieQueryOptions } from "../../services/movies";

export const Route = createFileRoute("/movies/$movieId")({
  loader: (opts) =>
    opts.context.queryClient.ensureQueryData(
      findMovieQueryOptions(opts.params.movieId),
    ),
  component: Movie,
});

function Movie() {
  const { movieId } = Route.useParams();
  const movieQuery = useSuspenseQuery(findMovieQueryOptions(movieId));

  return (
    <>
      <div>{`Hello /movies/${movieId}!`}</div>
      <pre>{JSON.stringify(movieQuery.data, null, 2)}</pre>
    </>
  );
}
