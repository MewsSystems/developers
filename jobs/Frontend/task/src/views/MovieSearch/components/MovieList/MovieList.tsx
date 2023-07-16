import { FC } from "react";
import { tablet } from "src/styles/appSize";
import { Movie } from "src/views/MovieSearch/components/MovieList/components/Movie";
import { MovieListProps } from "src/views/MovieSearch/components/MovieList/MovieListProps";
import styled, { css } from "styled-components";

export const MovieList: FC<MovieListProps> = (props) => {
  if (!props.movies) return null;
  if (props?.movies && !props?.movies?.length)
    return <NotFound>Oops! No movies found, search again!</NotFound>;

  return (
    <Wrap data-testid={"movieList"}>
      {props.movies.map((movie) => (
        <Movie key={movie.id} movie={movie} />
      ))}
    </Wrap>
  );
};

const Wrap = styled.div`
  width: 100%;
  margin-top: 24px;
  display: grid;
  background: #333333;
  padding: 24px 0;
  grid-template-columns: repeat(auto-fit, minmax(210px, 210px));
  grid-gap: 24px;
  justify-content: center;
  border-radius: 4px;
  box-shadow: 8px 8px 10px 0px rgba(0, 0, 0, 0.3);

  ${tablet(css`
    padding: 20px;
  `)}
`;

const NotFound = styled.span`
  margin-top: 12px;
`;
