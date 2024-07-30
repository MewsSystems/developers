import { tmdb, Crew } from "@/app/services/tmdb";
import { useSuspenseQuery } from "@tanstack/react-query";

export function useMovieData(id: string) {
  const idAsNumber = Number(id);

  const { data: movie } = useSuspenseQuery({
    queryKey: ["movie-detail", id],
    queryFn: async () => tmdb.movies.details(idAsNumber, ["credits"]),
  });

  return {
    movie,
    cast: movie.credits.cast,
    crew: movie.credits.crew,
  };
}

export function getDirectors(crew: Crew[]) {
  return crew?.filter((member) => member.job === "Director");
}
