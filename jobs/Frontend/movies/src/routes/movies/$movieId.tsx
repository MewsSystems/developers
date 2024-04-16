import { createFileRoute } from "@tanstack/react-router";
import { useSuspenseQuery } from "@tanstack/react-query";
import { findMovieQueryOptions } from "../../services/movies";
import { MovieDetail } from "../../components/MovieDetail";
import styled from "styled-components";

const StyledContainer = styled.div`
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  padding: 42px 32px;
`;

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
    <StyledContainer>
      <MovieDetail movie={movieQuery.data} />
    </StyledContainer>
  );
}
