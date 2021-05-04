import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import styled from "styled-components";
import {
  getCategory,
  getGenres,
  getMovieImgSrc,
  getReleaseDate,
  getRevenue,
} from "../helpers";
import { MovieDetail, MovieDetailViewParams } from "../types";
import { movieDbApi } from "../api/movie-db-api";
import { IMAGE_URL, MOVIE_DB_IMG_WIDTH } from "../constants";

interface IMovieDetailViewProps {
  children?: never;
}

export const MovieDetailView: React.FC<IMovieDetailViewProps> = () => {
  const { movieId } = useParams<MovieDetailViewParams>();

  const [movieDetail, setMovieDetail] = useState<MovieDetail>();

  useEffect(() => {
    movieDbApi
      .getMovieDetail(movieId)
      .then((response) => setMovieDetail(response.data));
  }, [movieId]);

  return (
    <MovieDetailViewLayout>
      <ImgContainer>
        <Img
          src={getMovieImgSrc({
            baseImgUrl: IMAGE_URL.BASE_MOVIE_DB,
            placeholderUrl: IMAGE_URL.BASE_PLACEHOLDER,
            imgSrc: movieDetail?.poster_path,
            imgWidth: MOVIE_DB_IMG_WIDTH.PX_300,
          })}
          alt="movie preview"
        />
      </ImgContainer>
      <InfoContainer>
        <InfoWrapper>
          <Title>{movieDetail?.original_title}</Title>
        </InfoWrapper>
        <InfoWrapper>
          <h4> Score</h4>
          {`${movieDetail?.vote_average} \u002F 10  (${movieDetail?.vote_count})`}
        </InfoWrapper>
        <InfoWrapper>
          <h4> Genres</h4>
          {getGenres(movieDetail?.genres || [])}
        </InfoWrapper>
        <InfoWrapper>
          <h4>Initial release</h4>
          {getReleaseDate(movieDetail?.release_date)}
        </InfoWrapper>
        <InfoWrapper>
          <h4>Revenue</h4>
          {`${getRevenue(movieDetail?.revenue)}`}
        </InfoWrapper>
        <InfoWrapper>
          <h4>Category</h4>
          {getCategory(movieDetail?.adult)}
        </InfoWrapper>
      </InfoContainer>
      <OverviewContainer>
        <Title>Overview</Title>
        {movieDetail?.overview}
      </OverviewContainer>
    </MovieDetailViewLayout>
  );
};

const MovieDetailViewLayout = styled.div`
  width: 100%;
  height: 100vh;
  padding: 1rem;
  display: grid;
  grid-template-columns: max-content 1fr;
  grid-template-rows: max-content 1fr;
  grid-template-areas:
    "img desc"
    "overview overview";
  gap: 1rem;
  background: ${(props) => props.theme.color.grey};
`;

const ImgContainer = styled.div`
  grid-area: img;
  display: block;
`;
const InfoContainer = styled.div`
  grid-area: desc;
  padding: 40px;
  border: ${({ theme }) => `1px solid ${theme.color.grey2}`};
  background: ${(props) => props.theme.color.white};
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 100%;
`;

const InfoWrapper = styled.div`
  border-bottom: ${({ theme }) => `1px solid ${theme.color.grey2}`};
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
`;

const OverviewContainer = styled.div`
  grid-area: overview;
  text-align: justify;
  line-height: 1.6;
`;

const Img = styled.img`
  object-fit: fill;
  width: 100%;
  height: 100%;
`;
const Title = styled.h3`
  margin-top: 0;
`;
