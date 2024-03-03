import MovieSearch from "@/scenes/MovieSearch/MovieSearch";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "Movie Search",
};

export default function Home() {
  return <MovieSearch />;
}
