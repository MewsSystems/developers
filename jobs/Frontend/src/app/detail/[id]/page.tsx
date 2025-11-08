import MovieDetail from "@/scenes/MovieDetail/MovieDetail";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "Movie Detail",
};

export default function DetailPage({
  params: { id: movieId },
}: {
  params: { id: string };
}) {
  return <MovieDetail movieId={movieId} />;
}
