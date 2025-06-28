import { useState, useRef } from "react";
import { type Movie } from "../../api/types";
import { GridCard } from "../../components/Grid/GridCard";
import {
  MovieCardImage,
  MovieCardTitle,
  MovieCardHoveredContent,
  MovieMetaContainer,
} from "./MovieCard.internal";
import { getImageUrl, getYearFromDate } from "./MovieCard.helpers";

interface MovieCardProps {
  movieData: Movie;
}

export const MovieCard = (props: MovieCardProps) => {
  const [hoveredCardId, setHoveredCardId] = useState<Movie["id"] | null>(null);
  const hoverTimeoutRef = useRef<number | null>(null);

  const handleCardHover = (movieId: number | null) => {
    if (hoverTimeoutRef.current) {
      clearTimeout(hoverTimeoutRef.current);
      hoverTimeoutRef.current = null;
    }

    if (movieId === null) {
      setHoveredCardId(null);
    } else {
      hoverTimeoutRef.current = setTimeout(() => {
        setHoveredCardId(movieId);
      }, 500);
    }
  };

  return (
    <GridCard
      item={props.movieData}
      onClick={() => console.log("clicked")}
      $isHovered={hoveredCardId === props.movieData.id}
      onMouseEnter={() => handleCardHover(props.movieData.id)}
      onMouseLeave={() => handleCardHover(null)}
    >
      <MovieCardImage
        src={getImageUrl(props.movieData.poster_path || "")}
        alt={`Poster of ${props.movieData.original_title}`}
        loading="lazy"
      />

      {hoveredCardId === props.movieData.id ? (
        <MovieCardHoveredContent>
          <MovieMetaContainer>
            <MovieCardTitle>{props.movieData.original_title}</MovieCardTitle>
            <MovieCardTitle>
              {getYearFromDate(props.movieData.release_date)}
            </MovieCardTitle>
            {props.movieData.adult ? (
              <MovieCardTitle>+18</MovieCardTitle>
            ) : null}
            <MovieCardTitle>
              {props.movieData.vote_average.toFixed(1)}
            </MovieCardTitle>
          </MovieMetaContainer>
        </MovieCardHoveredContent>
      ) : null}
    </GridCard>
  );
};
