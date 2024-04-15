import { createFileRoute } from "@tanstack/react-router";
import z from "zod";
import { searchMoviesQueryOptions } from "../services/movies";
import { useSuspenseQuery } from "@tanstack/react-query";

const movieSearchSchema = z.object({
  page: z.number().catch(1),
  query: z.string().catch(""),
});

export const Route = createFileRoute("/")({
  validateSearch: movieSearchSchema,
  loaderDeps: ({ search }) => ({ ...search }),
  loader: (opts) =>
    opts.context.queryClient.ensureQueryData(
      searchMoviesQueryOptions(opts.deps),
    ),
  component: Home,
});

function Home() {
  const { page, query } = Route.useSearch();
  const moviesQuery = useSuspenseQuery(
    searchMoviesQueryOptions(Route.useLoaderDeps()),
  );

  return (
    <div>
      <h3>Welcome Home!</h3>
      <span>{page}</span>
      <span>{query}</span>
      <pre>{JSON.stringify(moviesQuery.data, null, 2)}</pre>
    </div>
  );
}
