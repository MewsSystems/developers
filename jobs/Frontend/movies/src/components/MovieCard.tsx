import React from "react";
import { Link } from "@tanstack/react-router";
import styled from "styled-components";
import { MovieResult } from "../services/movies";

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
  background: darkgray;
  border-radius: 8px 8px 0px 0px;
  text-align: center;
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
        <h2 title={movie.title}>{movie.title}</h2>
      </StyledTitleContainer>
      <StyledCardBody>
        <StyledImgContainer>
          <StyledImg src={movie.imgSrc} alt={movie.title} />
        </StyledImgContainer>

        <StyledMovieInfoContainer>
          <StyledOverview title={movie.overview}>
            {movie.overview}
          </StyledOverview>
          <h5>{movie.rating.toFixed(1)} out of 10</h5>
        </StyledMovieInfoContainer>
      </StyledCardBody>
    </StyledLink>
  );
};
