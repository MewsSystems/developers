import React from "react";
import { Link } from "@tanstack/react-router";
import styled from "styled-components";
import { MovieResult } from "../services/movies";
import { media } from "../styles/breakpoints";

const StyledLink = styled(Link)`
  display: flex;
  flex-direction: column;
  text-decoration: none;
  color: inherit;
  background-color: white;
  border-radius: 8px;
  padding; 8px;
`;

const StyledTitleContainer = styled.div`
  background: var(--clr-blue-500);
  border-radius: 8px 8px 0px 0px;
  text-align: center;
  padding: 12px 8px;
`;

const StyledTitle = styled.h1`
  font-family: var(--ff-serif);
  font-size: var(--fs-400);
  color: var(--clr-blue-100);
  font-weight: bold;
  line-height: 1.75rem;

  ${media.lg`
    font-size: var(--fs-300);
  `}
`;

const StyledImgContainer = styled.div`
  flex: 1 1 0;
`;

const StyledImg = styled.img`
  aspect-ratio: 2/3;
  width: 100%;
  border-radius: 8px;
`;

const StyledCardBody = styled.div`
  display: flex;
  margin: 12px 14px;
`;

const StyledMovieInfoContainer = styled.div`
  display: flex;
  flex-direction: column;
  flex: 2 1 0;
  padding: 12px;
`;

const StyledOverview = styled.h4`
  overflow: hidden;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 5;
  font-size: var(--fs-400);
  font-family: var(--ff-serif);
  color: var(--clr-slate-700);
`;

const StyledRating = styled.h5`
  margin-top: 12px;
  font-size: var(--fs-300);
  color: var(--clr-blue-400);
  font-family: var(--ff-sans);
`;

export const MovieCard: React.FC<{ movie: MovieResult }> = ({
  movie,
  ...props
}) => {
  return (
    <StyledLink
      to="/movies/$movieId"
      params={{
        movieId: movie.id,
      }}
      {...props}
    >
      <StyledTitleContainer>
        <StyledTitle title={movie.title}>{movie.title}</StyledTitle>
      </StyledTitleContainer>
      <StyledCardBody>
        <StyledImgContainer>
          <StyledImg src={movie.imgSrc} alt={movie.title} />
        </StyledImgContainer>

        <StyledMovieInfoContainer>
          <StyledOverview title={movie.overview}>
            {movie.overview}
          </StyledOverview>
          <StyledRating>{movie.rating.toFixed(1)} out of 10</StyledRating>
        </StyledMovieInfoContainer>
      </StyledCardBody>
    </StyledLink>
  );
};
