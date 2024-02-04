import { Box } from "@mui/material";
import React, { useEffect, useRef, useState } from "react";
import MovieSearchInput from "../components/MovieSearchInput";
import MovieList from "../components/MovieList";
import useGetMovieList from "../customHooks/useGetMovieList";

const MovieSearch = () => {
  const [inputValue, setInputValue] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const timer = useRef<number | null>(null);
  const { movieList, maxPage, loading, setQuery } =
    useGetMovieList(currentPage);

  useEffect(() => {
    if (timer.current) {
      clearTimeout(timer.current);
    }

    timer.current = window.setTimeout(() => {
      setQuery(inputValue);
    }, 500);

    return () => {
      if (timer.current) {
        clearTimeout(timer.current);
      }
    };
  }, [inputValue, setQuery]);

  const handleInputChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setInputValue(event.target.value);
    setCurrentPage(1);
  };

  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        gap: "2rem",
        paddingTop: "2rem",
      }}
    >
      <MovieSearchInput
        inputValue={inputValue}
        onChange={handleInputChange}
      />
      <MovieList
        movieList={movieList}
        setCurrentPage={setCurrentPage}
        currentPage={currentPage}
        maxPage={maxPage}
        loading={loading}
      />
    </Box>
  );
};

export default MovieSearch;
