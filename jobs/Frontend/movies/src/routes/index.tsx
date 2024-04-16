import { useState, useEffect } from "react";
import { createFileRoute, useNavigate } from "@tanstack/react-router";
import z from "zod";
import { useSuspenseQuery } from "@tanstack/react-query";
import styled from "styled-components";

import { searchMoviesQueryOptions } from "../services/movies";
import { SearchInput } from "../components/SearchInput";
import { MovieCard } from "../components/MovieCard";

const HomeContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: 12px;
  margin: 8px 12px;
`;

const MoviesContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  grid-auto-rows: 1fr;
  gap: 8px;
`;

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
  const navigate = useNavigate({ from: Route.fullPath });
  const { page, query } = Route.useSearch();
  const moviesQuery = useSuspenseQuery(
    searchMoviesQueryOptions(Route.useLoaderDeps()),
  );
  const [queryDraft, setQueryDraft] = useState(query);

  useEffect(() => {
    void navigate({
      search: (old) => ({
        ...old,
        query: queryDraft,
      }),
    });
  }, [navigate, queryDraft]);

  return (
    <HomeContainer>
      <h3>Welcome Home!</h3>
      <p>{page}</p>
      <SearchInput
        placeholder="Search"
        value={queryDraft}
        onChange={(e) => {
          setQueryDraft(e.target.value);
        }}
      />
      <MoviesContainer>
        {moviesQuery.data.movies.map((movie) => (
          <MovieCard movie={movie} key={movie.id} />
        ))}
      </MoviesContainer>
    </HomeContainer>
  );
}
