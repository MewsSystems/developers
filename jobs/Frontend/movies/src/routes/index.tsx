import { useState, useEffect } from "react";
import { createFileRoute, useNavigate, Link } from "@tanstack/react-router";
import z from "zod";
import { useSuspenseQuery } from "@tanstack/react-query";
import styled from "styled-components";

import { searchMoviesQueryOptions } from "../services/movies/searchMovies";
import { SearchInput } from "../components/SearchInput";
import { MovieCard } from "../components/MovieCard";
import { Pagination } from "../components/Pagination";
import { media } from "../styles/breakpoints";

const HomeContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: 12px;
  margin: 8px 12px;
`;

const StyledIntroContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap; 12px;
  justify-content: center;
  align-items: center;
  padding: 18px 24px;
  
  ${media.lg`
    padding: 12px 16px;
  `};
`;

const StyledWelcome = styled.h1`
  font-size: var(--fs-700);
  color: var(--clr-slate-900);
  font-family: var(--ff-serif);

  ${media.lg`
    font-size: var(--fs-600); 
  `};
`;

const StyledInstructions = styled.p`
  font-size: var(--fs-500);
  color: var(--clr-blue-700);
  font-family: var(--ff-sand);

  ${media.lg`
    font-size: var(--fs-400); 
  `};
`;

const MoviesContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  grid-auto-rows: 1fr;
  gap: 10px;
  padding-top: 8px;

  ${media.sm`
    grid-template-columns: repeat(1, minmax(0, 1fr));
  `}
`;

const StyledLink = styled(Link)`
  display: flex;
  text-decoration: none;
`;

const movieSearchSchema = z.object({
  page: z.number().catch(1),
  query: z.string().catch(""),
});

export const Route = createFileRoute("/")({
  validateSearch: movieSearchSchema,
  loaderDeps: ({ search }) => ({ ...search }),
  loader: (opts) => opts.context.queryClient.ensureQueryData(searchMoviesQueryOptions(opts.deps)),
  component: Home,
});

function Home() {
  const navigate = useNavigate({ from: Route.fullPath });
  const { query } = Route.useSearch();
  const moviesQuery = useSuspenseQuery(searchMoviesQueryOptions(Route.useLoaderDeps()));
  const [queryDraft, setQueryDraft] = useState(query);

  useEffect(() => {
    void navigate({
      search: (old) => ({
        ...old,
        query: queryDraft,
        page: 1,
      }),
    });
  }, [navigate, queryDraft]);

  const onPageChange = (newPage: number) => {
    void navigate({
      search: (old) => ({
        ...old,
        page: newPage,
      }),
    });
  };

  return (
    <HomeContainer>
      <StyledIntroContainer>
        <StyledWelcome>Welcome to Mews Movies assigment web</StyledWelcome>
        <StyledInstructions>
          Search your favorite movie and click to see more details
        </StyledInstructions>
      </StyledIntroContainer>

      <SearchInput
        placeholder="Search movie"
        value={queryDraft}
        onChange={(e) => {
          setQueryDraft(e.target.value);
        }}
      />
      <MoviesContainer>
        {moviesQuery.data.movies.map((movie) => (
          <StyledLink
            key={movie.id}
            to="/movies/$movieId"
            params={{
              movieId: movie.id,
            }}
          >
            <MovieCard movie={movie} />
          </StyledLink>
        ))}
      </MoviesContainer>
      <Pagination
        currentPage={moviesQuery.data.page}
        totalPages={moviesQuery.data.totalPages}
        onPageChange={onPageChange}
      />
    </HomeContainer>
  );
}
