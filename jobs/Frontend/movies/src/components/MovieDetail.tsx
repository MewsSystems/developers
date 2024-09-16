import React from "react";
import styled from "styled-components";
import { Movie } from "../services/movies/findMovieById";
import { media } from "../styles/breakpoints";

const StyledContainer = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
  background: white;
  border-radius: 8px;
  padding-bottom: 28px;
`;

const StyledHeroContainer = styled.div<{ $imageSrc: string }>`
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  align-items: center;
  height: 350px;
  position: relative;
  overflow: hidden;
  flex: 1 1 auto;
  z-index: 1;
  border-top-left-radius: 8px;
  border-top-right-radius: 8px;

  color: white;

  &:before {
    content: "";
    width: 100%;
    height: 100%;
    position: absolute;
    overflow: hidden;
    top: 0;
    left: 0;
    background: url(${(props) => props.$imageSrc});
    background-size: cover;
    z-index: -1;
  }
`;
const StyledHeroContentContainer = styled.div`
  display: flex;
  text-shadow: 0 2px 3px rgba(0, 0, 0, 0.4);
  background: linear-gradient(to top, rgba(0, 0, 0, 0.85), transparent);
  flex-direction: column;
  width: 100%;
  align-items: center;
  padding: 18px 12px;
`;

const StyledHeroTitle = styled.h1`
  font-size: var(--fs-500);
  font-family: var(--ff-serif);
  color: var(--clr-slate-100);
  font-weight: bold;
  text-wrap: pretty;
`;

const StyledHeroInfoContainer = styled.div`
  display: flex;
  gap: 12px;
  margin-top: 16px;
  align-items: center;
  justify-content: center;
`;

const StyledRating = styled.h3`
  font-size: var(--fs-400);
  font-family: var(--ff-sans);
  color: var(--clr-slate-200);
  font-weight: 900;
`;

const StyledRuntime = styled.h3`
  font-size: var(--fs-300);
  font-family: var(--ff-sans);
  color: var(--clr-slate-200);
  font-weight: 200;
`;

const StyledHeroGendersContainer = styled.div`
  display: flex;
  margin-top: 14px;
  gap: 8px;
`;

const StyledGender = styled.span`
  background: var(--clr-slate-50);
  color: var(--clr-slate-800);
  border-radius: 10px;
  padding: 2px 8px;
  font-size: var(--fs-200);
  font-family: var(--ff-serif);
  line-height: 20px;
`;

const StyledContentContainer = styled.div`
  display: flex;
  flex-direction: column;
  max-width: 720px;
  margin: 0px auto;
  padding: 14px 12px;
`;

const StyledOverview = styled.p`
  font-family: var(--ff-sans);
  font-size: 20px;
  color: var(--clr-slate-800);

  ${media.sm`
    font-size: var(--fs-400);
  `}
`;

const StyledProduction = styled.span`
  font-family; var(--ff-serif);
  font-size: var(--fs-300);
  color: var(--clr-slate-700);
`;

export interface MovieDetailProps {
  movie: Movie;
}

export const MovieDetail: React.FC<MovieDetailProps> = ({ movie }) => {
  return (
    <StyledContainer>
      <StyledHeroContainer $imageSrc={movie.imgSrc}>
        <StyledHeroContentContainer>
          <StyledHeroTitle data-testid="movieDetailTitle">{movie.title}</StyledHeroTitle>
          <StyledHeroInfoContainer>
            <StyledRating data-testid="movieDetailRating">{`⭐️ ${movie.rating.toFixed(1)}`}</StyledRating>
            <StyledRuntime data-testid="movieDetailRuntime">{`⏱️ ${movie.runtime} minutes`}</StyledRuntime>
          </StyledHeroInfoContainer>
          <StyledHeroGendersContainer>
            {movie.genres.map((gender, index) => (
              <StyledGender data-testid="movieDetailGender" key={index}>
                {gender}
              </StyledGender>
            ))}
          </StyledHeroGendersContainer>
        </StyledHeroContentContainer>
      </StyledHeroContainer>
      <StyledContentContainer>
        <StyledOverview data-testid="movieDetailOverview">{movie.overview}</StyledOverview>
        <StyledProduction data-testid="movieDetailProduction">
          {movie.productionCompanies.join(", ")}
        </StyledProduction>
      </StyledContentContainer>
    </StyledContainer>
  );
};
