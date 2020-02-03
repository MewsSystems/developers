import React from "react";
import MovieListItem from "./MovieListItem";
import { List, Header } from "../styles/Style.js";
import { storeSelectedMovieID } from "../Helpers/ReduxHelper";
import { useSelector, useDispatch } from "react-redux";

const MovieList = () => {
  const movieData = useSelector(state => state.movieData);
  const dispatch = useDispatch();

  if (movieData.status !== 200) return <div />;

  const onClickHandler = id => {
    dispatch(storeSelectedMovieID(id));
  };

  const movieObjects = movieData.data.results.map(movie => (
    <MovieListItem
      key={movie.id}
      movie={movie}
      clickHandler={onClickHandler.bind(this, movie.id)}
    />
  ));

  return (
    <List>
      {movieObjects.length === 0 && (
        <Header>We could not find any movie with this name...</Header>
      )}
      {movieObjects.length > 0 && movieObjects}
    </List>
  );
};

export default MovieList;
