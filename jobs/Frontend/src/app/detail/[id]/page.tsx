import MovieDetail from "@/scenes/MovieDetail/MovieDetail";

export default function DetailPage({
  params: { id: movieId },
}: {
  params: { id: string };
}) {
  return <MovieDetail movieId={movieId} />;
}
