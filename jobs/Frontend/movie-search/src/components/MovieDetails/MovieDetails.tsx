"use client";

import { BaseMovieDetails } from "@/interfaces/MovieInterfaces";
import { runtimeToHoursMinutes } from "@/utils/RuntimeToMinutes";
import {
  MainContainer,
  Title,
  Text,
  MoviePoster,
} from "./MovieDetailsStyledComponents";

interface MovieDeatilsPageProps {
  movie: BaseMovieDetails;
}

export default function MovieDetails({ movie }: MovieDeatilsPageProps) {
  return (
    <MainContainer data-testid="movieDetails">
      <Title>{movie.title}</Title>
      <Text>{movie.overview}</Text>
      <Text>Release Date: {movie.release_date}</Text>
      {movie.poster_path && (
        <MoviePoster
          src={`https://image.tmdb.org/t/p/w200${movie.poster_path}`}
          alt={`${movie.title} poster`}
        />
      )}
      <Text>Runtime: {runtimeToHoursMinutes(movie.runtime)}</Text>
    </MainContainer>
  );
}
