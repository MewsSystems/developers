import React from "react";
import Box from "@mui/material/Box";
import { MovieCard } from "./components/MovieCard";
import { Movie } from "../../../../types";

type MovieGridProps = {
  data: Movie[];
}

export const MovieGrid = ({ data }: MovieGridProps) => {
  return (
    <Box display="flex" flexWrap="wrap" gap={2}>
      {data.map(movie => (
        <MovieCard
          key={movie.id}
          id={movie.id} 
          path={movie.path}
          title={movie.title}
        />
      ))}
    </Box>
  );
};
