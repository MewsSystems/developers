import { useParams } from "react-router-dom";
import { useEffect } from "react";
import { useMovieContext } from "../../contexts/MovieContext";
import {
  Container,
  Title,
  PosterWrapper,
  Poster,
  Info,
  BackLink,
  SkeletonContainer,
  SkeletonBlock,
  SkeletonPoster,
} from "./MovieDetailsStyles";

export const MovieDetails = () => {
  const { movieId } = useParams<{ movieId: string }>();
  const { getMovieInfo, movieDetails, loading, currentPage } =
    useMovieContext();

  useEffect(() => {
    if (movieId) {
      getMovieInfo(Number(movieId));
    }
  }, [movieId]);

  if (loading) {
    return (
      <SkeletonContainer>
        <SkeletonBlock width="60%" height="2rem" /> {/* Title */}
        <SkeletonPoster /> {/* Poster */}
        <SkeletonBlock width="80%" />
        <SkeletonBlock width="90%" />
        <SkeletonBlock width="70%" />
        <SkeletonBlock width="50%" />
      </SkeletonContainer>
    );
  }

  return (
    <Container>
      <BackLink to={`/movies/${currentPage}`}>← Back to Movie List</BackLink>
      <Title>{movieDetails.title}</Title>
      <PosterWrapper>
        <Poster
          src={`https://image.tmdb.org/t/p/w500${movieDetails.poster_path}`}
          alt={movieDetails.title}
        />
      </PosterWrapper>
      <Info>
        <strong>Overview:</strong> {movieDetails.overview}
      </Info>
      <Info>
        <strong>Release Date:</strong> {movieDetails.release_date}
      </Info>
      <Info>
        <strong>Run Time:</strong> {movieDetails.runtime} minutes
      </Info>
      <Info>
        <strong>Rating:</strong> {movieDetails.vote_average}
      </Info>
    </Container>
  );
};
