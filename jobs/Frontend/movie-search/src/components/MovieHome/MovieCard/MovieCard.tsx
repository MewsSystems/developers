"use client";
import Link from "next/link";
import {
  MovieCardContainer,
  Poster,
  ReleaseDate,
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
    <Link key={movie.id} href={`/movie/${movie.id}?fromSearch=true`}>
      <MovieCardContainer>
        <Poster
          src={`https://image.tmdb.org/t/p/w200${movie.posterPath}`}
          alt={`${movie.title} poster`}
        />
        <Title data-testid={`movieTitle${index}`}>{movie.title}</Title>
        <ReleaseDate>{getYear(movie.releaseDate)}</ReleaseDate>
      </MovieCardContainer>
    </Link>
  );
}
