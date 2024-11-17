import { useGetMovie } from "./useGetMovie";
import { imagesEndpoint } from "../../config";
import { extractYearFromReleaseDate } from "../../utils";
import { LoadingIndicator } from "@/components/ui/loading-indicator";
import { ImageWithPlaceholder } from "./ImageWithPlaceholder";
import { Frown } from "lucide-react";

export type MovieDetailProps = {
  movieId: number;
};

/**
 * Movie detail component.
 * Fetches movie details data and renders it.
 * While data is being loaded, renders a loading message.
 * When data fetching ends with an error, renders an error message.
 */
export const MovieDetail: React.FC<MovieDetailProps> = ({ movieId }) => {
  const { data: movie, isPending, isError } = useGetMovie({ movieId });

  if (isError) {
    return <ErrorMessage />;
  }

  if (isPending) {
    return <LoadingIndicator text="Fetching movie details..." />;
  }

  const moviePosterUrl = `${imagesEndpoint}${movie.poster_path}`;
  const genres = movie.genres?.map((genre) => genre.name).join(" / ");
  const productionCountries = movie.production_countries?.map((country) => country.name).join(" / ");

  return (
    <div className="gap-4">
      <h1 className="pb-4 font-bold text-2xl">{movie.original_title}</h1>
      <div className="flex gap-4 h-[375px]">
        <ImageWithPlaceholder
          src={moviePosterUrl}
          alt={`${movie.original_title} poster`}
          className="hidden md:block"
          loadingPlaceholder={
            <div className="flex justify-center items-center min-w-[250px]">Loading image...</div>
          }
          errorPlaceholder={<div className="flex justify-center items-center min-w-[250px]">No image</div>}
        />
        <div>
          <div className="flex mb-2 font-medium">{genres || "Genres unknown"}</div>
          <div className="flex mb-2 font-medium">
            {productionCountries || "Production countries unknown"},{" "}
            {extractYearFromReleaseDate(movie.release_date) || "Release year unknown"},{" "}
            {movie.runtime ? `${movie.runtime} min` : "Runtime unknown"}
          </div>
          <p>{movie.overview}</p>
        </div>
      </div>
    </div>
  );
};

export const ErrorMessage = () => {
  return (
    <div className="flex justify-center items-center">
      <Frown className="mr-2" />
      Unexpected error.
    </div>
  );
};
