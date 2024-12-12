import React from "react";
import styled from "styled-components";
import { DetailedMovie } from "../../../models/tmdbModels";
import { MovieData } from "../../atoms/movie/movie-data";
import { MoviePosterCard } from "../../atoms/movie/movie-poster-card";
import { TitleAndSubtitle } from "../../atoms/movie/title-subtitle";
import { MoviePunctuation } from "../../atoms/movie/movie-punctuation";
import { DetailedMovieDataGrid } from "../../molecules/movie/detailed-movie-data-grid";

export const MovieDetails: React.FC<{ detailedMovie: DetailedMovie }> = ({
  detailedMovie,
}) => {
  return (
    <Container
      backdrop={`https://image.tmdb.org/t/p/original${detailedMovie.backdrop_path}`}
    >
      <Overlay />

      <Content>
        <PosterContainer>
          <MoviePosterCard movie={detailedMovie} />
        </PosterContainer>

        <DetailsContainer>
          <TitleAndSubtitle
            title={detailedMovie.title}
            subtitle={detailedMovie.tagline}
          />
          <InfoRow>
            <MoviePunctuation
              avg={detailedMovie.vote_average}
              totalVotes={detailedMovie.vote_count}
            />
            <Runtime>Runtime: {detailedMovie.runtime} mins</Runtime>
          </InfoRow>
          <Overview>{detailedMovie.overview}</Overview>
          <MovieData
            title="Production Companies"
            content={
              <ul>
                {detailedMovie.production_companies.map((company) => (
                  <li key={company.id}>
                    {company.name} ({company.origin_country})
                  </li>
                ))}
              </ul>
            }
          />

          <DetailedMovieDataGrid detailedMovie={detailedMovie} />
        </DetailsContainer>
      </Content>
    </Container>
  );
};

const Container = styled.div<{ backdrop: string }>`
  position: relative;
  width: 100%;
  height: 100%;
  color: white;
  background-image: url(${(props) => props.backdrop});
  background-size: cover;
  background-position: center;
`;

const Overlay = styled.div`
  position: absolute;
  inset: 0;
  background: linear-gradient(
    to bottom,
    rgba(0, 0, 0, 0.9),
    rgba(0, 0, 0, 0.8),
    black
  );
`;

const Content = styled.div`
  position: relative;
  z-index: 10;
  display: flex;
  flex-direction: column;
  padding: 2rem;
  max-width: 72rem;
  margin: 0 auto;
  height: 100%;
  padding-top: 150px;

  @media (min-width: 768px) {
    flex-direction: row;
  }
`;

const PosterContainer = styled.div`
  flex-shrink: 0;
  width: 100%;
  height: 33%;

  @media (min-width: 768px) {
    width: 33%;
  }
`;

const DetailsContainer = styled.div`
  margin-top: 1.5rem;

  @media (min-width: 768px) {
    margin-top: 0;
    margin-left: 2rem;
    flex: 1;
  }
`;

const InfoRow = styled.div`
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-top: 1rem;
`;

const Runtime = styled.span`
  font-size: 0.875rem;
  color: #b3b3b3;
`;

const Overview = styled.p`
  margin-top: 1.5rem;
  color: #b3b3b3;
`;
