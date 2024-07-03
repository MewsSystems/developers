import React, { useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import styled from "styled-components";
import { useMovies } from "../hooks/useMovies";

const Container = styled.div`
  padding: 16px;
`;

const MovieDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const { movieDetail, getMovieDetail, loading, error } = useMovies();

  useEffect(() => {
    if (!id) return;
    getMovieDetail(id);
  }, [id, getMovieDetail]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  if (!movieDetail) return <div>No movie found.</div>;

  return (
    <Container>
      <h1>{movieDetail.title}</h1>
      <Link to="/">
        <button>Go Back</button>
      </Link>
      <p>{movieDetail.overview}</p>
      <img
        src={`https://image.tmdb.org/t/p/w500${movieDetail.poster_path}`}
        alt={movieDetail.title}
      />
    </Container>
  );
};

export default MovieDetailPage;
