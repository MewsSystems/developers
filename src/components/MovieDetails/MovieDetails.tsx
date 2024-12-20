import React from "react";
import { useMemo } from "react";
import styled from "styled-components";
import { Movie } from "../Form/Form";
import { Genre } from "../../hooks/useGenres/useGenres";
import MissingPoster from "../Form/MissingPoster.jpg"


export interface MovieDetailsProps {
  movie: Movie; 
  onBackToList: () => void; 
  genres: Genre[];
}

const MovieItemWrapper = styled.div`
  margin: 20px;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 4px;
  width: 60%;
  font-size: 2vmin;
  display: flex;
  flex-direction: row;
  max-height: 60vh;
`;

const MovieData = styled.div`
  display: flex;
  flex-direction: column;
  margin: 0 20px;
  justify-content: start;
  align-items: start;
  text-align: left;
`;

const MovieTitle = styled.h2`
  margin: 0;
`;

const Poster = styled.img`
  height: 30vmin;
`;

const Rating = styled.div`
  display: flex;
  align-items: baseline;
  gap: 1vmin;
`;

const BackToListButton = styled.button`
  padding: 1vmin 2vmin;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 25px;
  cursor: pointer;
  margin-bottom: 5vmin;
  font-size: calc(5px + 2vmin);

  &:hover {
    background-color: #ff6a00;
  }
`;

export const MovieDetails: React.FC<MovieDetailsProps> = ({ movie, onBackToList, genres }):JSX.Element => {
  const genreNames = useMemo(()=>
    movie.genre_ids.map((id) => {
    const genre = genres.find((g) => g.id === id);
    return genre ? genre.name : "Unknown";
  }),[movie.genre_ids, genres]);
  return (
    <>
      <MovieItemWrapper>
        <Poster src={movie.poster_path? `https://image.tmdb.org/t/p/w500/${movie.poster_path}`:MissingPoster} alt={`Poster ${movie.title}`} />
        <MovieData>
          <MovieTitle>{movie.title}</MovieTitle>
          <p>Release date: {movie.release_date}</p>
          <h4>Genres: {genreNames.join(", ")}</h4>
          <p>{movie.overview}</p>
          <Rating>
            <h4>Rating:</h4>
            <h2>{movie.vote_average}</h2> based on <h4>{movie.vote_count}</h4>{" "}
            reviews
          </Rating>
        </MovieData>
      </MovieItemWrapper>
      <BackToListButton onClick={onBackToList}>Back to list</BackToListButton>
    </>
  );
};