import React, { useEffect, useState } from 'react';
import { MovieDetail as MovieDetailType } from '../types';
import { getMovieDetails } from '../api';
import styled from 'styled-components';
import { useParams } from 'react-router-dom';

const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
`;

const Poster = styled.img`
  width: 300px;
  height: 450px;
  object-fit: cover;
  margin-bottom: 20px;
`;

const Title = styled.h1`
  font-size: 24px;
  margin-bottom: 10px;
`;

const Overview = styled.p`
  font-size: 16px;
  text-align: center;
  max-width: 600px;
`;

const Details = styled.div`
  font-size: 14px;
  text-align: center;
`;

const MovieDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [movie, setMovie] = useState<MovieDetailType | null>(null);

  useEffect(() => {
    const fetchMovieDetails = async () => {
      const data = await getMovieDetails(Number(id));
      setMovie(data);
    };

    fetchMovieDetails();
  }, [id]);

  if (!movie) {
    return <p>Loading...</p>;
  }

  return (
    <Container>
      <Poster src={`https://image.tmdb.org/t/p/w300${movie.poster_path}`} alt={movie.title} />
      <Title>{movie.title}</Title>
      <Overview>{movie.overview}</Overview>
      <Details>
        <p>Genres: {movie.genres.map((genre) => genre.name).join(', ')}</p>
        <p>Runtime: {movie.runtime} minutes</p>
        <p>Rating: {movie.vote_average}</p>
        <p>Release Date: {movie.release_date}</p>
      </Details>
    </Container>
  );
};

export default MovieDetail;