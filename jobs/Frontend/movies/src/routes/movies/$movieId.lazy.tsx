import { createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/movies/$movieId")({
  component: Movie,
});

function Movie() {
  const { movieId } = Route.useParams();
  return <div>{`Hello /movies/${movieId}!`}</div>;
}
