import React from "react";
import { Movie } from "../types/types";
import MovieCard from "./MovieCard";
import {
  Box,
  Button,
  CircularProgress,
} from "@mui/material";

type MovieListProps = {
  movieList: Movie[];
  setCurrentPage: React.Dispatch<
    React.SetStateAction<number>
  >;
  currentPage: number;
  maxPage: number;
  loading: boolean;
};

const MovieList: React.FC<MovieListProps> = ({
  movieList,
  setCurrentPage,
  currentPage,
  maxPage,
  loading,
}) => {
  return (
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        flexWrap: "wrap",
        gap: "1.5rem",
        width: "100%",
      }}
    >
      {movieList.map((movie) => (
        <MovieCard key={movie.id} movieData={movie} />
      ))}
      {movieList.length > 0 && currentPage < maxPage && (
        <Box
          sx={{
            width: "100%",
            display: "flex",
            justifyContent: "center",
            margin: "2rem",
          }}
        >
          <Button
            variant="contained"
            onClick={() =>
              setCurrentPage((prev) => prev + 1)
            }
          >
            Load more movies
          </Button>
        </Box>
      )}
      {loading && (
        <Box
          sx={{
            width: "100%",
            display: "flex",
            justifyContent: "center",
          }}
        >
          <CircularProgress />
        </Box>
      )}
    </Box>
  );
};

export default MovieList;
