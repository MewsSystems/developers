import { FC } from "react";
import { MovieInfoProps } from "src/views/MovieSearch/components/MovieList/components/MovieInfoProps";
import styled from "styled-components";

export const MovieInfo: FC<MovieInfoProps> = (props) => {
  return (
    <div>
      <h3>{props.title}</h3>
      <MovieOverView>{props.description}</MovieOverView>
    </div>
  );
};

const MovieOverView = styled.div`
  height: 70%;
`;
