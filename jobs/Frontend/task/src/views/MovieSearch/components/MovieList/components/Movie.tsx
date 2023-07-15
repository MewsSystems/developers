import { FC } from "react";
import { Link } from "react-router-dom";
import { Route } from "src/router/Route";
import { MovieInfo } from "src/views/MovieSearch/components/MovieList/components/MovieInfo";
import { MovieProps } from "src/views/MovieSearch/components/MovieList/components/MovieProps";
import styled, { css } from "styled-components";

export const Movie: FC<MovieProps> = (props) => {
  const movie = props.movie;
  const posterPath = movie.poster_path;
  const bgLink = `https://image.tmdb.org/t/p/w${
    props.imgSize || "200"
  }${posterPath}`;

  if (props.disableLink)
    return (
      posterPath && (
        <MovieWrap width={props.imgSize}>
          <Image src={bgLink} alt="movieBg" />
        </MovieWrap>
      )
    );

  return (
    <LinkedMovieWrap to={`${Route.Movie}/${movie.id}`}>
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
    </LinkedMovieWrap>
  );
};

const MovieWrapStyle = css`
  width: 210px;
  height: 300px;
  display: inline-flex;
  transition: 0.3s;
  box-shadow: 4px 4px 10px 0px rgba(0, 0, 0, 0.3);
  cursor: pointer;

  &:hover {
    transform: scale(1.035);
  }

  color: white;
  text-decoration: none;
`;

const MovieContentStyle = css`
  width: 210px;
  z-index: 10;
  transition: 0.3s;
  background: rgba(0, 0, 0, 0.75);
  padding: 12px;
  max-height: 290px;
  overflow-y: scroll;
  padding-bottom: 16px;
  border-bottom: 18px solid transparent;
`;

const LinkedMovieWrap = styled(Link)`
  ${MovieWrapStyle}
`;

// Covers wrap when disableLink is true
const MovieWrap = styled.div`
  ${MovieWrapStyle}
  width: ${(props) => (props.width ? props.width : 200)}px;
  height: ${(props) => (props.width ? props.width * 1.5 : 300)}px;
  cursor: initial;

  &:hover {
    transform: none;
  }

  img {
    width: ${(props) => (props.width ? props.width : 200)}px;
    height: ${(props) => (props.width ? props.width * 1.5 : 300)}px;
    position: initial;
  }
`;

const Image = styled.img`
  width: 210px;
  height: 300px;
  position: absolute;
  z-index: 1;
`;

const MovieContent = styled.div`
  ${MovieContentStyle}

  h3 {
    margin-top: 0;
    height: 30%;
  }
`;

const MovieContentHover = styled.div`
  ${MovieContentStyle}
  opacity: 0;

  &:hover {
    opacity: 1;
  }

  h3 {
    margin-top: 0;
  }
`;
