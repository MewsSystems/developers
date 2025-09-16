import { useQueryMovieDetails } from "@/pages/movie-details/hooks/useQueryMovieDetails";
import { DetailsComponent } from "@/pages/movie-details/ui";
import { Box } from "@chakra-ui/react";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute('/_auth/moviedetails/$movieId' as any)({
  component: RouteComponent,
});

function RouteComponent() {
  const { movieId } = Route.useParams();
  const { data: details, isLoading, isError, error } = useQueryMovieDetails({ movie_id: movieId })
  if (isLoading) {
    return <div>LOADING</div>
  }
  if (isError) {
    console.error(error);
    return <Box>Sorry, error</Box>
  }
  return <div>
    {details?.movie && <DetailsComponent detailsProps={details} />}
  </div>
}
