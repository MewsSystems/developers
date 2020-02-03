import React, { useRef } from "react";
import { Input, Button, VerticalDiv } from "../styles/Style.js";
import MovieList from "./MovieList";
import { storeSearchBoxValue, storeMovieData } from "../Helpers/ReduxHelper";
import { useSelector, useDispatch } from "react-redux";
import APIHelper from "../Helpers/APIHelper.js";

const MovieSearch = () => {
  const searchBoxRef = useRef(null);
  const movieData = useSelector(state => state.movieData);
  const searchBoxValue = useSelector(state => state.searchBoxValue);

  const dispatch = useDispatch();

  const onSearchButtonClick = () => {
    const searchBoxValue = searchBoxRef.current && searchBoxRef.current.value;

    dispatch(storeSearchBoxValue(searchBoxValue));
    fetchMovies(searchBoxValue, 1);
  };

  const fetchMovies = async (searchParam, page) => {
    const response = await APIHelper.fetchMovies(searchParam, page);
    dispatch(storeMovieData(response));
  };

  const onPagingButtonClick = page => {
    fetchMovies(searchBoxValue, page);
    window.document.documentElement.scrollTop = 0;
  };

  const getNextButton = () => {
    return (
      movieData.data &&
      movieData.data.total_pages > 0 &&
      movieData.data.page !== movieData.data.total_pages && (
        <Button
          onClick={onPagingButtonClick.bind(this, movieData.data.page + 1)}
        >
          Next
        </Button>
      )
    );
  };

  const getPreviousButton = () => {
    return (
      movieData.data &&
      movieData.data.page !== 1 && (
        <Button
          onClick={onPagingButtonClick.bind(this, movieData.data.page - 1)}
        >
          Previous
        </Button>
      )
    );
  };

  return (
    <React.Fragment>
      <VerticalDiv>
        <Input
          type="text"
          placeholder="Search for movies..."
          ref={searchBoxRef}
        ></Input>
        <Button onClick={onSearchButtonClick}>Search</Button>
      </VerticalDiv>
      <MovieList />
      <div style={{ display: "flex", "justify-content": "flex-end" }}>
        {getPreviousButton()}
        {getNextButton()}
      </div>
    </React.Fragment>
  );
};

export default MovieSearch;
