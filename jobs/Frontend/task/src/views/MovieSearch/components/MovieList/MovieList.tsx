import { FC } from "react";
import { useGetMoviesQuery } from "src/store/slices/moviesSlice";
import { MovieType } from "src/store/types/MovieType";
import { ReduxHookReturn } from "src/store/types/ReduxHookReturn";
import { tablet } from "src/styles/appSize";
import { Movie } from "src/views/MovieSearch/components/MovieList/components/Movie";
import { MovieListProps } from "src/views/MovieSearch/components/MovieList/MovieListProps";
import styled, { css } from "styled-components";

export const MovieList: FC<MovieListProps> = (props) => {
  const { data }: ReduxHookReturn<MovieType[]> = useGetMoviesQuery(
    props.inputValue,
    { skip: props.inputValue === "" },
  );

  const movies = data?.results || [];

  return (
    <Wrap>
      {movies.map((movie) => (
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
  padding: 24px 40px;
  grid-template-columns: repeat(auto-fit, minmax(210px, 210px));
  grid-gap: 24px;
  justify-content: center;

  ${tablet(css`
    padding: 20px;
  `)}
`;
