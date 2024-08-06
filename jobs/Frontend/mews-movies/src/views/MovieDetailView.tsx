import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import styled from "styled-components";
import { getMovieDetails, getMovieImageUrl } from "../api/tmdb";
import { MovieDetails } from "../types/MovieInterfaces";
import { handleBackNavigation } from "../utils/navigationUtils";

const MovieDetailContainer = styled.div`
  padding: 2rem;
  display: flex;
  flex-direction: row;
  align-items: flex-start;
  justify-content: center;
`;

const MoviePoster = styled.img`
  width: 300px;
  border-radius: 15px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
  height: auto;
  margin-right: 2rem;
`;

const MovieDetailText = styled.div`
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  line-height: 1.5;
`;

const MovieTitle = styled.h1`
  font-size: 2rem;
  margin-bottom: 1rem;
`;

const MovieOverview = styled.p`
  font-size: 1rem;
  margin-bottom: 1rem;
  max-width: 800px;
  text-align: left;
`;

const MovieDetailView: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [movie, setMovie] = useState<MovieDetails | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchMovie = async () => {
      setError(null);
      try {
        const data = await getMovieDetails(id!);
        setMovie(data);
      } catch (err) {
        setError("Error fetching movie details");
      }
    };

    fetchMovie();
  }, [id]);

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Backspace") {
        handleBackNavigation(navigate);
      }
    };

    window.addEventListener("keydown", handleKeyDown);

    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [navigate]);

  if (error) {
    return <p>{error}</p>;
  }

  if (!movie) {
    return <p>Loading...</p>;
  }

  return (
    <MovieDetailContainer>
      <MoviePoster
        src={getMovieImageUrl(movie.poster_path)}
        alt={movie.title}
      />
      <MovieDetailText>
        <MovieTitle>{movie.title}</MovieTitle>
        <MovieOverview>{movie.overview}</MovieOverview>
        <p>Release Date: {movie.release_date}</p>
        <p>Rating: {movie.vote_average}</p>
        <p>Number of votes: {movie.vote_count}</p>
        <p>Original language: {movie.original_language}</p>
      </MovieDetailText>
    </MovieDetailContainer>
  );
};

export default MovieDetailView;
