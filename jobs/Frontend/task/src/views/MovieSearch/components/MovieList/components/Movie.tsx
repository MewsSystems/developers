import { FC } from "react";
import { MovieInfo } from "src/views/MovieSearch/components/MovieList/components/MovieInfo";
import { MovieProps } from "src/views/MovieSearch/components/MovieList/components/MovieProps";
import styled from "styled-components";

export const Movie: FC<MovieProps> = (props) => {
  const movie = props.movie;
  const posterPath = movie.poster_path;
  const bgLink = `https://image.tmdb.org/t/p/w200${posterPath}`;

  return (
    <MovieWrap>
      {posterPath && <Image src={bgLink} alt="movieBg" />}
      {posterPath ? (
        <MovieContentHover>
          <MovieInfo title={movie.title} description={movie.overview} />
        </MovieContentHover>
      ) : (
        <MovieContent>
          <MovieInfo title={movie.title} description={movie.overview} />
        </MovieContent>
      )}
    </MovieWrap>
  );
};

const MovieWrap = styled.div`
  height: 300px;
  display: inline-flex;
  transition: 0.3s;
  box-shadow: 4px 4px 10px 0px rgba(0, 0, 0, 0.3);
  cursor: pointer;

  &:hover {
    transform: scale(1.035);
  }
`;

const Image = styled.img`
  width: 210px;
  height: 300px;
  position: absolute;
  z-index: 1;
`;

const MovieContent = styled.div`
  width: 210px;
  z-index: 10;
  transition: 0.3s;
  background: rgba(0, 0, 0, 0.6);
  padding: 12px;
  max-height: 290px;
  overflow-y: scroll;
  padding-bottom: 16px;
  border-bottom: 18px solid transparent;

  h3 {
    margin-top: 0;
    height: 30%;
  }
`;

const MovieContentHover = styled.div`
  width: 210px;
  z-index: 10;
  opacity: 0;
  transition: 0.3s;
  background: rgba(0, 0, 0, 0.75);
  padding: 12px;
  max-height: 290px;
  overflow-y: scroll;
  padding-bottom: 16px;
  border-bottom: 18px solid transparent;

  &:hover {
    opacity: 1;
  }

  h3 {
    margin-top: 0;
  }
`;

