import { Movie, useGetMovie } from "./useGetMovie";
import { imagesEndpoint } from "../../config";
import { extractYearFromReleaseDate } from "../../utils";
import { ImageWithPlaceholder } from "./ImageWithPlaceholder";
import { Frown, Loader2, X } from "lucide-react";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import React, { PropsWithChildren } from "react";

export type MovieDetailProps = {
  movieId: number;
  movieTitle: string;
  onClose: () => void;
};

/**
 * Movie detail component.
 * Fetches movie details data and renders it in a dialog.
 * While data is being loaded, renders a dialog with a movie title and a loading message.
 * When data fetching ends with an error, renders a dialog with a movie title and an error message.
 */
export const MovieDetail: React.FC<MovieDetailProps> = ({ movieId, movieTitle, onClose }) => {
  const { data: movie, isPending, isError } = useGetMovie({ movieId });

  if (isError) {
    return (
      <DialogComponent onClose={onClose} title={movieTitle}>
        <ErrorMessage />
      </DialogComponent>
    );
  }

  if (isPending || !movie) {
    return (
      <DialogComponent onClose={onClose} title={movieTitle}>
        <PendingMessage />
      </DialogComponent>
    );
  }

  return (
    <DialogComponent onClose={onClose} title={movieTitle}>
      <MovieDetailContent movie={movie} />
    </DialogComponent>
  );
};

type MovieDetailContentProps = {
  movie: Movie;
};

const MovieDetailContent: React.FC<MovieDetailContentProps> = ({ movie }) => {
  const moviePosterUrl = `${imagesEndpoint}${movie.poster_path}`;
  const genres = movie.genres?.map((genre) => genre.name).join(" / ");
  const productionCountries = movie.production_countries?.map((country) => country.name).join(" / ");

  return (
    <div className="flex gap-4 h-full">
      <ImageWithPlaceholder
        src={moviePosterUrl}
        alt={`${movie.original_title} poster`}
        className="hidden md:block"
        loadingPlaceholder={
          <div className="center-items min-w-[250px]">
            <Loader2 className="animate-spin mr-2" />
            Loading image...
          </div>
        }
        errorPlaceholder={
          <div className="center-items min-w-[250px]">
            <X className="mr-2" />
            No image
          </div>
        }
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
  );
};

const ErrorMessage = () => {
  return (
    <div className="center-items h-full">
      <Frown className="mr-2" />
      Ooops, something went wrong.
    </div>
  );
};

const PendingMessage = () => {
  return (
    <div className="center-items h-full">
      <Loader2 className="animate-spin mr-2" />
      Fetching movie details...
    </div>
  );
};

type DialogComponent = PropsWithChildren & {
  title: string;
  onClose: () => void;
};

const DialogComponent: React.FC<DialogComponent> = ({ title, onClose, children }) => {
  return (
    <Dialog defaultOpen onOpenChange={onClose}>
      <DialogContent className="w-11/12 max-w-[1000px]">
        <DialogHeader>
          <DialogTitle className="text-xl">{title}</DialogTitle>
        </DialogHeader>
        <div className="h-[375px]">{children}</div>
      </DialogContent>
    </Dialog>
  );
};
