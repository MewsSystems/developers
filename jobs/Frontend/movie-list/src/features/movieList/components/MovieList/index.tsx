import { FC } from "react";
import { useGetSearchMoviesQuery } from "../../hooks";
import type { MovieList as MovieListType } from "../../types";
import { MovieListItem } from "./parts/MovieListItem";
import { List } from "./styled";

export const MovieList: FC = () => {
  const { data, isLoading } = useGetSearchMoviesQuery({
    query: "avengers",
    page: 1,
  });
  const movies = (data?.results || []) as MovieListType;

  if (isLoading) {
    return <div>Results are loading</div>;
  }

  if (!data) {
    return <div>Error loading movie list</div>;
  }

  return (
    <>
      <h1>The Movie List</h1>
      <input
        type="text"
        id="movie-search-input"
        placeholder="Start typing to view movie list"
      />
      <List>
        {movies.length &&
          movies.map((movieItem) => {
            return (
              <MovieListItem movie={movieItem} key={movieItem.id.toString()} />
            );
          })}
      </List>
    </>
  );
};
