import React, { useEffect, useState } from "react";
import { ImdbMovieResponse, Movie } from "../../../models/tmdbModels";
import { MovieCarrousel } from "../../molecules/movie/movie-carrousel";
import { tmdbService } from "../../../services/tmdbServie";
import styled from "styled-components";

const homeMoviesTopics = ["Now Playing", "Top Rated", "Upcoming"];

export const MainTopicsMoviesCarrousels: React.FC = () => {
  const [homeMovies, setHomeMovies] = useState<
    { title: string; movies: Movie[] }[]
  >([]);

  const [errorMessage, setErrorMessage] = useState<null | string>(null);

  const fetchHomeListMovies = async () => {
    try {
      const fetchPromises = homeMoviesTopics.map((topic) =>
        tmdbService.get<ImdbMovieResponse<Movie[]>>(
          `/3/movie/${topic
            .replaceAll(" ", "_")
            .toLowerCase()}?language=en-US&page=1`
        )
      );
      const results = await Promise.all(fetchPromises);
      let moviesData: { title: string; movies: Movie[] }[] = [];
      results.forEach((response, index) => {
        if (response.results.length > 0) {
          moviesData.push({
            title: homeMoviesTopics[index],
            movies: response.results,
          });
        }
      });
      setErrorMessage(null);
      setHomeMovies(moviesData);
    } catch (error) {
      setErrorMessage("Failed to fetch movies. Please try again.");
    }
  };

  useEffect(() => {
    fetchHomeListMovies();
  }, []);

  if (errorMessage) {
    <ErrorMessage>{errorMessage}</ErrorMessage>;
  }
  return (
    <>
      {homeMovies.map((moviesWithTitle, index) => (
        <MovieCarrousel
          key={moviesWithTitle.title + index}
          movies={moviesWithTitle.movies}
          title={moviesWithTitle.title}
        />
      ))}
    </>
  );
};

const ErrorMessage = styled.div`
  color: red;
  font-weight: bold;
`;
