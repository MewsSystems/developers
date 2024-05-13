"use client";

import { BaseMovieDetails } from "@/interfaces/MovieInterfaces";
import { runtimeToHoursMinutes } from "@/utils/RuntimeToMinutes";
import {
  MainContainer,
  Title,
  Text,
  MoviePoster,
} from "./MovieDetailsStyledComponents";
import { formatDateString } from "@/utils/FormatDateString";

interface MovieDeatilsPageProps {
  movie: BaseMovieDetails;
}

export default function MovieDetails({ movie }: MovieDeatilsPageProps) {
  return (
    <MainContainer data-testid="movieDetails">
      <Title>{movie.title}</Title>
      <Text>{movie.overview}</Text>
      <Text>Release Date: {formatDateString(movie.release_date)}</Text>
      <Text>Runtime: {runtimeToHoursMinutes(movie.runtime)}</Text>
      <Text>Genres: {movie.genres.map((genre) => genre.name).join(", ")}</Text>
      {movie.poster_path && (
        <MoviePoster
          src={`https://image.tmdb.org/t/p/w200${movie.poster_path}`}
          alt={`${movie.title} poster`}
        />
      )}
    </MainContainer>
  );
}
