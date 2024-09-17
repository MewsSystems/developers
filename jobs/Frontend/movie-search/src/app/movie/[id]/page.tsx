import MovieDetails from "@/components/MovieDetails/MovieDetails";
import MovieNavigation from "@/components/MovieDetails/MovieNavigation/MovieNavigation";
import { fetchSingleMovie } from "@/data/FetchSingleMovie";

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
    <>
      <MovieNavigation isFromSearch={searchParams.fromSearch} />
      <MovieDetails movie={data} />
    </>
  );
}
