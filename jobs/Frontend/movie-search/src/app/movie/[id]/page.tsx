import { Metadata } from "next";
import MovieNavigation from "@/components/MovieNavigation/MovieNavigation";
import { fetchSingleMovie } from "@/data/FetchSingleMovie";
import { runtimeToHoursMinutes } from "@/utils/RuntimeToMinutes";

export const metadata: Metadata = {
  title: "MEWS Movie Search - Movie Details",
  description: "Movie Details",
};

interface MovieDeatilsPageProps {
  params: { id: number };
  searchParams: { fromSearch: boolean };
}

export default async function MovieDetailsPage({
  params,
  searchParams,
}: MovieDeatilsPageProps) {
  const data = await fetchSingleMovie("/movie/", params.id);
  return (
    <main>
      <MovieNavigation isFromSearch={searchParams.fromSearch} />
      <h1>{data.title}</h1>
      <p>{data.overview}</p>
      <p>{data.release_date}</p>
      {data.poster_path && (
        <img
          src={`https://image.tmdb.org/t/p/w200${data.poster_path}`}
          alt={`${data.title} poster`}
        />
      )}
      <p>{runtimeToHoursMinutes(data.runtime)}</p>
    </main>
  );
}
