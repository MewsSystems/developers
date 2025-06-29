import { useState, useRef } from "react";
import { type Movie } from "../../api/types";
import { GridCard } from "../../components/Grid/GridCard";
import { MovieCardHoveredData } from "./MovieCard.internal";
import { getImageUrl } from "../../utils/movieHelpers";
import ImagePlaceholder from "../../assets/no-image-placeholder.jpg";
import { Image } from "../../components/Image/Image";

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
      $item={props.movieData}
      onClick={() => console.log("clicked")}
      $isHovered={hoveredCardId === props.movieData.id}
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

      {hoveredCardId === props.movieData.id ? (
        <MovieCardHoveredData movieData={props.movieData} />
      ) : null}
    </GridCard>
  );
};
