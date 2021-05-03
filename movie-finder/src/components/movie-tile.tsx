import React from "react";
import styled from "styled-components";
import { Button } from "./button";
import { useHistory } from "react-router-dom";
import { Routes } from "./app-router";

export interface IMovieTileProps {
  movieId: number;
  originalTitle: string;
  overview: string;
  voteAverage: number;
  voteCount: number;
  posterSrc: string | null;
  children?: never;
}

const getMovieImgSrc = (imgSrc: string): string =>
  !imgSrc.endsWith("null") ? imgSrc : "http://via.placeholder.com/185x280";

export const MovieTile: React.FC<IMovieTileProps> = (props) => {
  const {
    movieId,
    originalTitle,
    overview,
    voteAverage,
    voteCount,
    posterSrc,
  } = props;

  const history = useHistory();

  const handleGetMovieDetailClick = () => {
    history.push(Routes.DETAIL.replace(":movieId", `${movieId}`));
  };

  return (
    <TileContainer>
      <ImgContainer>
        <Img
          src={getMovieImgSrc(`https://image.tmdb.org/t/p/w185/${posterSrc}`)}
          alt="movie preview"
        />
      </ImgContainer>
      <Title>{originalTitle}</Title>
      <Description>{overview}</Description>
      <Score>{`Score ${voteAverage} \u002F 10  (${voteCount})`}</Score>
      <ActionContainer>
        <Button
          onClick={handleGetMovieDetailClick}
          content="Detail"
          variant="primary"
          isDisabled={false}
        />
      </ActionContainer>
    </TileContainer>
  );
};

const TileContainer = styled.div`
  border-radius: 4px;
  width: 100%;
  background: ${(props) => props.theme.color.white};
  font-size: 1rem;
  display: grid;
  gap: 1rem;
  grid-template-columns: max-content 1fr min-content;
  grid-template-rows: 1fr 8fr 2fr;
  grid-template-areas:
    "img title title"
    "img desc desc"
    "img score action";
  box-shadow: rgba(100, 100, 111, 0.2) 0 7px 29px 0;
  padding: 1rem;
  overflow: hidden;
`;

const Title = styled.div`
  grid-area: title;
  display: flex;
  font-size: ${(props) => props.theme.fontSize.large};
`;

const ImgContainer = styled.div`
  grid-area: img;
`;

const Description = styled.div`
  grid-area: desc;
  font-size: ${(props) => props.theme.fontSize.small};
  color: ${(props) => props.theme.color.black};
  opacity: 0.5;
  overflow: hidden;
  width: 100%;
  height: 100%;
  text-overflow: ellipsis;
  line-height: 1.3;
  letter-spacing: 0.01em;
`;

const Score = styled.div`
  grid-area: score;
  font-size: ${(props) => props.theme.fontSize.large};
  align-self: center;
  white-space: nowrap;
`;

const ActionContainer = styled.div`
  grid-area: action;
  display: flex;
  justify-content: flex-end;
  align-self: center;
`;
const Img = styled.img`
  object-fit: scale-down;
  width: 100%;
  height: 100%;
`;
