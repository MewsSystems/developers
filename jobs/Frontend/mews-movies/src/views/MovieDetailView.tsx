import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import styled from "styled-components";
import { getMovieDetails, getMovieImageUrl } from '../api/tmdb';
import { MovieDetails } from '../types/Movie';

const MovieDetailContainer = styled.div`
  padding: 2rem;
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const MoviePoster = styled.img`
  width: 300px;
  height: auto;
  margin-bottom: 1rem;
`;

const MovieTitle = styled.h1`
  font-size: 2rem;
  margin-bottom: 1rem;
`;

const MovieOverview = styled.p`
  font-size: 1rem;
  margin-bottom: 1rem;
  max-width: 800px;
  text-align: center;
`;

const MovieDetailView: React.FC = () => {
  const { id } = useParams<{ id: string }>();
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

  if (error) {
    return <p>{error}</p>;
  }

  if (!movie) {
    return <p>Loading...</p>;
  }

  return (
    <MovieDetailContainer>
      <MoviePoster src={getMovieImageUrl(movie.poster_path)} alt={movie.title} />
      <MovieTitle>{movie.title}</MovieTitle>
      <MovieOverview>{movie.overview}</MovieOverview>
      <p>Release Date: {movie.release_date}</p>
      <p>Rating: {movie.vote_average}</p>
    </MovieDetailContainer>
  );
};

export default MovieDetailView;
