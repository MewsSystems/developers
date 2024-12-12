import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { DetailedMovie } from "../../models/tmdbModels";
import { tmdbService } from "../../services/tmdbServie";
import { CircularProgress } from "@mui/material";
import { useAppDispatch } from "../../redux/reduxHooks";
import { setMovieSearched } from "../../redux/searchEngine";
import { MovieDetails } from "../organisms/movie/movie-details";
import styled from "styled-components";

export const MoviePage: React.FC = () => {
  const route = useParams<{ movieId: string }>();
  const dispatch = useAppDispatch();
  const [errorMessage, setErrorMessage] = useState<null | string>(null);
  const [movie, setMovie] = useState<DetailedMovie>();

  const getMovie = async () => {
    await tmdbService
      .get<DetailedMovie>(`/3/movie/${route.movieId}`, true)
      .then((response) => {
        if (response) {
          setMovie(response);
        }
      })
      .catch((error: Error) => {
        setErrorMessage(error.message);
      });
  };

  useEffect(() => {
    dispatch(setMovieSearched(""));
    getMovie();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (!movie) {
    return (
      <CircularProgressContainer>
        <CircularProgress />
      </CircularProgressContainer>
    );
  }
  if (errorMessage) {
    return <ErrorMessage>{errorMessage}</ErrorMessage>;
  } else
    return (
      <>
        <MovieDetails detailedMovie={movie} />
      </>
    );
};

const CircularProgressContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100vh;
`;

const ErrorMessage = styled.div`
  color: red;
  font-weight: bold;
`;
