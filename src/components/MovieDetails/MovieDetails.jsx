import React from "react";
import styled from "styled-components";

const MovieItemWrapper = styled.div`
  margin: 20px;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 4px;
  width: 60%;
  font-size: 2vmin;
  display: flex;
  flex-direction: row;
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

export const MovieDetails = ({ movie, onBackToList }) => {
  return (
    <>
      <MovieItemWrapper>
        <Poster src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`} />
        <MovieData>
          <MovieTitle>{movie.title}</MovieTitle>
          <p>Release date: {movie.release_date}</p>
          <p>{movie.overview}</p>
        </MovieData>
      </MovieItemWrapper>
      <BackToListButton onClick={onBackToList}>Back to list</BackToListButton>
    </>
  );
};
