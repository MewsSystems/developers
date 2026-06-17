import { createFileRoute, useRouter } from "@tanstack/react-router";
import { useSuspenseQuery } from "@tanstack/react-query";
import styled from "styled-components";

import { findMovieByIdQueryOptions } from "../../services/movies/findMovieById";
import { MovieDetail } from "../../components/MovieDetail";

const StyledContainer = styled.div`
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  padding: 42px 32px;
  gap: 12px;
`;

const StyledBackButton = styled.button`
  font-family: var(--ff-serif);
  color: var(--clr-blue-400);
  border-radius: 8px;
  border: 1px solid var(--clr-blue-400);
  background: transparent;
  max-width: 150px;
  padding: 12px 8px;
  cursor: pointer;

  &:hover {
    background: var(--clr-blue-100);
  }
`;

export const Route = createFileRoute("/movies/$movieId")({
  loader: (opts) =>
    opts.context.queryClient.ensureQueryData(findMovieByIdQueryOptions(opts.params.movieId)),
  component: Movie,
});

function Movie() {
  const router = useRouter();
  const { movieId } = Route.useParams();
  const movieQuery = useSuspenseQuery(findMovieByIdQueryOptions(movieId));
  return (
    <StyledContainer>
      <StyledBackButton
        onClick={() => {
          router.history.back();
        }}
      >
        ← Back
      </StyledBackButton>
      <MovieDetail movie={movieQuery.data} />
    </StyledContainer>
  );
}
