"use client";
import Link from "next/link";
import {
  MovieCardContainer,
  Poster,
  ReleaseDate,
  StyledLink,
  Title,
} from "./MovieCardStyledComponents";
import getYear from "@/utils/GetYear";

export interface MovieCardProps {
  movie: {
    id: number;
    title: string;
    releaseDate: string;
    posterPath: string | null;
  };
  index: number;
}

export default function MovieCard({ movie, index }: MovieCardProps) {
  return (
    <StyledLink key={movie.id} href={`/movie/${movie.id}?fromSearch=true`}>
      <MovieCardContainer>
        {movie.posterPath ? (
          <Poster
            as="img"
            src={`https://image.tmdb.org/t/p/w200${movie.posterPath}`}
            alt={`${movie.title} poster`}
          />
        ) : (
          <Poster>No Image Available</Poster>
        )}
        <Title data-testid={`movieTitle${index}`}>{movie.title}</Title>
        <ReleaseDate>{getYear(movie.releaseDate)}</ReleaseDate>
      </MovieCardContainer>
    </StyledLink>
  );
}
