import { Link } from "react-router-dom";
import styled from "styled-components";
import { Image } from "@/shared/ui/atoms/Image";
import type { Movie } from "../types";

type MovieCardProps = {
  movie: Movie;
};

const Wrap = styled.div`
  padding: 1rem 0;
`

export function MovieCard({ movie }: MovieCardProps) {
  return (
    <Wrap>
      <Link to={`/movie/${movie.id}`}>
        <Image src={movie.posterPath} alt={`${movie.title} poster`} loading="lazy" />
      </Link>
    </Wrap>
  );
}
