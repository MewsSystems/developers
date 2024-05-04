import { Metadata } from "next";
import { constructMovieDbUrl } from "@/utils/movieDbUrl.util";
import { MoviesDbMovieDetailsResponse } from "@/interfaces/MoviesDb/moviesDbMovieDetailsResponse";
import { MovieDetail } from "@/interfaces/movie";
import MovieDetailsContainer from "@/components/MovieDetails";

export const metadata: Metadata = {
  title: "Details",
};

const fetchData = async (id: string) => {
  const movieUrl = constructMovieDbUrl(`/movie/${id}`);
  const movieResponse = await fetch(movieUrl);

  if (!movieResponse.ok) {
    // Logging here will be server-side not client, so it is not exposed. Additionally, we could log to a service we may use
    console.error(
      "Could not retrieve the movie details. Error: ",
      movieResponse.status,
      movieResponse.statusText,
    );

    // Don't return any movie instead of erroring
    return null;
  }

  const movieResponseData =
    (await movieResponse.json()) as MoviesDbMovieDetailsResponse;

  // Map response so we don't return any unnecessary information
  const movieData: MovieDetail = {
    genres: movieResponseData.genres,
    id: movieResponseData.id,
    originalLanguage: movieResponseData.original_language,
    originalTitle: movieResponseData.original_title,
    overview: movieResponseData.overview,
    posterUrl: movieResponseData.poster_path,
    productionCompanies: movieResponseData.production_companies.map(
      (productionCompany) => ({
        id: productionCompany.id,
        name: productionCompany.name,
        originCountry: productionCompany.origin_country,
      }),
    ),
    releaseDate: movieResponseData.release_date,
    runtime: movieResponseData.runtime,
    tagline: movieResponseData.tagline,
    title: movieResponseData.title,
    status: movieResponseData.status,
  };

  console.log(movieData); // TODO - remove

  return movieData;
};

interface MoviePageProps {
  params: { id: string };
}

export default async function Page({ params }: MoviePageProps) {
  const movie = await fetchData(params.id);

  if (movie) {
    return (
      <MovieDetailsContainer
        genres={movie.genres}
        originalLanguage={movie.originalLanguage}
        originalTitle={movie.originalTitle}
        overview={movie.overview}
        productionCompanies={movie.productionCompanies}
        posterUrl={movie.posterUrl}
        releaseDate={movie.releaseDate}
        runtime={movie.runtime}
        status={movie.status}
        tagline={movie.tagline}
        title={movie.title}
      />
    );
  }

  return <>No movie for this ID</>;
}
