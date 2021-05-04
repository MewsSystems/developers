import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import { API_KEY } from "../constants";
import styled from "styled-components";
import {
  getCategory,
  getGenres,
  getMovieImgSrcBig,
  getReleaseDate,
  getRevenue,
} from "../helpers";
import { MovieDetail, MovieDetailViewParams } from "../types";

interface IMovieDetailViewProps {
  children?: never;
}

export const MovieDetailView: React.FC<IMovieDetailViewProps> = () => {
  const { movieId } = useParams<MovieDetailViewParams>();

  const [movieDetail, setMovieDetail] = useState<MovieDetail>();

  useEffect(() => {
    axios
      .get(
        `https://api.themoviedb.org/3/movie/${movieId}?api_key=${API_KEY}&language=en-US`
      )
      .then((response) => setMovieDetail(response.data));
  }, [movieId]);

  return (
    <MovieDetailViewLayout>
      <ImgContainer>
        <Img
          src={getMovieImgSrcBig(
            `https://image.tmdb.org/t/p/w300/${movieDetail?.poster_path}`
          )}
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
