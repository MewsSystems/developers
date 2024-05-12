import { Metadata } from "next";
import MoviePagination from "@/components/MoviePagination/MoviePagination";
import { fetchMovies } from "@/data/FetchMovies";
import MovieCard from "@/components/MovieCard/MovieCard";
import MovieCardContainer from "@/components/MovieCard/MovieCardContainer";
import MovieHeader from "@/components/MovieHeader/MovieHeader";
import NoMoviesComponent from "@/components/NoMovies/NoMovies";

export const metadata: Metadata = {
  title: "MEWS Movie Search",
  description: "Search for a movie",
};

interface HomePageProps {
  searchParams: { searchterm: string; page: number };
}

export default async function Home({ searchParams }: HomePageProps) {
  const { movies, totalPages } = await fetchMovies(
    "/search/movie",
    searchParams.searchterm,
    searchParams.page
  );

  return (
    <main>
      <MovieHeader />
      {!searchParams.searchterm && (
        <NoMoviesComponent
          message="Search For Movies"
          additionalText="Use the search bar to find movies"
        />
      )}
      {movies.length === 0 && searchParams.searchterm && (
        <NoMoviesComponent
          message="No movies found"
          additionalText="Try searching for another movie"
        />
      )}
      {searchParams.searchterm && movies.length > 0 && (
        <MovieCardContainer>
          {movies.map((movie, index) => (
            <MovieCard key={movie.id} index={index} movie={movie} />
          ))}
        </MovieCardContainer>
      )}

      {totalPages > 1 && <MoviePagination totalPages={totalPages} />}
    </main>
  );
}
