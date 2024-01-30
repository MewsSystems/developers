import React from "react";
import Header from "../Header/Header";
import { StyledMoviePage } from "./MoviePage.styles";
import MovieDetailModalOpener from "../MovieDetailModalOpener/MovieDetailModalOpener";
import MoviePageContent from "../MoviePageContent/MoviePageContent";

const MoviePage = () => {
  return (
    <StyledMoviePage>
      <Header />
      <MoviePageContent />
      <MovieDetailModalOpener />
    </StyledMoviePage>
  );
};

export default MoviePage;
