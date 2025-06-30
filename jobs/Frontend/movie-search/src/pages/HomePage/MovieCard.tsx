import { useState, useRef } from "react";
import { type Movie } from "../../api/types";
import { GridCard } from "../../components/Grid/GridCard";
import { MovieCardHoveredData } from "./MovieCard.internal";
import { getImageUrl, getMovieDetailRoute } from "../../utils/movieHelpers";
import ImagePlaceholder from "../../assets/no-image-placeholder.jpg";
import { Image } from "../../components/Image/Image";
import { useNavigate } from "react-router";
import useIsMobile from "../../hooks/useIsMobile";

interface MovieCardProps {
  movieData: Movie;
}

export const MovieCard = (props: MovieCardProps) => {
  const [hoveredCardId, setHoveredCardId] = useState<Movie["id"] | null>(null);
  const navigate = useNavigate();
  const isMobile = useIsMobile();
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
      $item={props.movieData}
      onClick={() => navigate(getMovieDetailRoute(props.movieData.id))}
      $isHovered={hoveredCardId === props.movieData.id && !isMobile}
      onMouseEnter={() => handleCardHover(props.movieData.id)}
      onMouseLeave={() => handleCardHover(null)}
    >
      <Image
        src={
          props.movieData.poster_path
            ? getImageUrl(props.movieData.poster_path)
            : ImagePlaceholder
        }
        alt={
          props.movieData.poster_path
            ? `Poster of ${props.movieData.original_title}`
            : `No image for ${props.movieData.original_title}`
        }
        $width="100%"
        loading="lazy"
      />

      {hoveredCardId === props.movieData.id && !isMobile ? (
        <MovieCardHoveredData movieData={props.movieData} />
      ) : null}
    </GridCard>
  );
};
