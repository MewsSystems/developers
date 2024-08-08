import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import styled from "styled-components";
import { getMovieDetails, getMovieImageUrl } from "../api/tmdb";
import { MovieDetails } from "../types/MovieInterfaces";
import { handleBackNavigation } from "../utils/navigationUtils";

const MovieDetailContainer = styled.div`
  padding-top: 3rem;
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
  max-width: 800px;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  line-height: 1.5;
  text-align: left;
  gap: 0.7rem;
`;

const MovieTitle = styled.h1`
  font-size: 2rem;
  margin-bottom: 0;
`;

const GenreItem = styled.span`
  background-color: #ebe9e9;
  border-radius: 5px;
  padding: 0.25rem 0.5rem;
  margin-right: 0.5rem;
`;

const joinByKey = <T, K extends keyof T>(source: T[], key: K): string => {
  return source.map((item) => item[key]).join(", ");
};

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

  const director = movie.credits.crew.find(
    (member) => member.job === "Director"
  );

  const castString = movie.credits.cast
    .slice(0, 10)
    .map(
      (castMember) => `${castMember.name}
     (${castMember.character})`
    )
    .join(", ");

  const languages = joinByKey(movie.spoken_languages, "english_name");

  return (
    <MovieDetailContainer>
      <MoviePoster
        src={getMovieImageUrl(movie.poster_path)}
        alt={movie.title}
      />
      <MovieDetailText>
        <MovieTitle>{movie.title}</MovieTitle>
        <p>{movie.overview}</p>
        <p>
          <strong>Release Date:</strong> {movie.release_date}
        </p>
        <p>
          <strong>Rating:</strong> {movie.vote_average}
        </p>
        <p>
          <strong>Number of votes:</strong> {movie.vote_count}
        </p>
        <p>
          <strong>Original language:</strong> {languages}
        </p>

        {director && (
          <p>
            <strong>Director:</strong> {director.name}
          </p>
        )}

        <div>
          <strong>Genres: </strong>
          {movie.genres.map((genre) => (
            <GenreItem key={genre.id}>{genre.name}</GenreItem>
          ))}
        </div>

        <p>
          <strong>Cast:</strong> {castString}
        </p>
      </MovieDetailText>
    </MovieDetailContainer>
  );
};

export default MovieDetailView;
