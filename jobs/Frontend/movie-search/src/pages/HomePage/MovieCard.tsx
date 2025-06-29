import { useState, useRef } from "react";
import { type Movie } from "../../api/types";
import { GridCard } from "../../components/Grid/GridCard";
import {
  MovieCardImage,
  MoveCardInfo,
  MovieCardHoveredContent,
  MovieMetaContainer,
  MovieBadgeInfo,
  MovieCardInfoContainer,
} from "./MovieCard.internal";
import {
  getImageUrl,
  getTranslatedTitle,
  getYearFromDate,
} from "./MovieCard.helpers";

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

  const movieTitle = getTranslatedTitle(
    props.movieData.original_language === "en",
    props.movieData.original_title,
    props.movieData.title
  );

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
          <MovieCardInfoContainer>
            <MoveCardInfo>{movieTitle}</MoveCardInfo>
            <MovieMetaContainer>
              <MovieBadgeInfo>
                <MoveCardInfo>
                  {getYearFromDate(props.movieData.release_date)}
                </MoveCardInfo>
              </MovieBadgeInfo>
              {props.movieData.adult ? (
                <MovieBadgeInfo>
                  <MoveCardInfo>+18</MoveCardInfo>
                </MovieBadgeInfo>
              ) : null}
              <MovieBadgeInfo>
                <MoveCardInfo>
                  {props.movieData.vote_average.toFixed(1)}
                </MoveCardInfo>
              </MovieBadgeInfo>
            </MovieMetaContainer>
          </MovieCardInfoContainer>
        </MovieCardHoveredContent>
      ) : null}
    </GridCard>
  );
};
