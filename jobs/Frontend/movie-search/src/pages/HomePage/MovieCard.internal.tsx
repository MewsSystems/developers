import styled from "styled-components";
import {
  getMovieDetailRoute,
  getTranslatedTitle,
  getYearFromDate,
} from "../../utils/movieHelpers";
import type { Movie } from "../../api/types";
import { Link } from "react-router";
import { Fullscreen } from "lucide-react";

export const MovieCardInfoContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: flex-start;
  gap: 0.4rem;
`;

export const MovieCardHeaderContainer = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
`;

export const MovieMetaContainer = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  gap: 0.2rem;
`;

export const MovieBadgeInfo = styled.div`
  padding: 0.1rem;
  border: 1px solid #333;
`;

export const MoveCardInfo = styled.p`
  font-size: 0.6rem;
  color: #333;
  font-weight: 700;
`;

export const MovieCardHoveredContent = styled.div`
  position: absolute;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  padding: 0.6rem;
  color: #333;
  width: 100%;
  min-height: 20%;
  height: fit-content;
  bottom: 0;
  left: 0;
  background-color: #fff;
`;

export const MovieCardLink = styled(Link)`
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: inherit;
  text-decoration: none;
  color: #333;
  font-size: 0.8rem;
  font-weight: 400;
  background-color: #eee;
  padding: 0.4rem;
  border-radius: 50px;

  transition: all 0.3s ease-out;

  cursor: pointer;

  &:hover {
    background-color: #ddd;
  }
`;

interface MovieCardHoveredDataProps {
  movieData: Movie;
}

export const MovieCardHoveredData = (props: MovieCardHoveredDataProps) => {
  const movieTitle = getTranslatedTitle(
    props.movieData.original_language === "en",
    props.movieData.original_title,
    props.movieData.title
  );

  return (
    <MovieCardHoveredContent>
      <MovieCardInfoContainer>
        <MovieCardHeaderContainer>
          <MoveCardInfo>{movieTitle}</MoveCardInfo>
          <MovieCardLink
            title="More information"
            to={getMovieDetailRoute(props.movieData.id)}
          >
            <Fullscreen size={12} color="#333" />
          </MovieCardLink>
        </MovieCardHeaderContainer>
        <MovieMetaContainer>
          {props.movieData.release_date ? (
            <MovieBadgeInfo>
              <MoveCardInfo>
                {getYearFromDate(props.movieData.release_date)}
              </MoveCardInfo>
            </MovieBadgeInfo>
          ) : null}
          {props.movieData.adult ? (
            <MovieBadgeInfo>
              <MoveCardInfo>18+</MoveCardInfo>
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
  );
};
