import { MovieDetailsRouteComponent } from "@/pages/movie-details/ui";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/moviedetails/$movieId" as any)({
  component: MovieDetailsRouteComponent,
});


