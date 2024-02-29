export default function DetailPage({
  params: { id: movieId },
}: {
  params: { id: string };
}) {
  return <h1>Movie detail for movie with id {movieId}</h1>;
}
