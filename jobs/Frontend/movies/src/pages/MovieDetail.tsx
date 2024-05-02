import styled from "styled-components";
import { useQuery } from "@tanstack/react-query";
import { backdropUrl, fetchMovieById } from "src/api/tmdb";
import Spinner from "src/components/Spinner";
import PosterImage from "src/components/PosterImage";
import { getFullYear } from "src/utils/date";

type MovieDetailProps = {
  id: string;
};

function MovieDetail({ id }: MovieDetailProps) {
  const {
    isPending,
    error,
    data: movie,
  } = useQuery({
    queryKey: ["movie", id],
    queryFn: () => fetchMovieById(id),
    // pass all errors to error boundary
    throwOnError: true,
  });

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <span>Error: {error.message}</span>;
  }

  return (
    <Wrapper>
      <PosterImage path={movie.poster_path} size="w500" />
      
      <div>
        <Title>
          {movie.title}
          <Year>({getFullYear(movie.release_date)})</Year>
        </Title>
        <Genres>{movie.genres.map((g) => g.name).join(", ")}</Genres>
      </div>

      <SectionTitle>Overview</SectionTitle>
      <Overview>{movie.overview}</Overview>

      <div>Score:{Math.round(movie.vote_average * 10)}%</div>
      <div>Voted:{movie.vote_count}</div>

      {movie.backdrop_path && (
        <img
          src={backdropUrl(movie.backdrop_path, "w1280")}
          alt="poster"
          role="presentation"
        />
      )}
      
    </Wrapper>
  );
}

const Wrapper = styled.div`
  display: grid;
  gap: 32px;
  padding-top: 16px;
  padding-bottom: 16px;
`;

const Title = styled.h1`
  color: var(--color-dark-text);
  font-size: 1.125rem;
  font-weight: 500;

  @media (min-width: 600px) {
    font-size: 1.875rem;
    font-weight: 700;
  }
`;

const Year = styled.span`
  margin-left: 0.5rem;
  font-weight: 300;
`;

const Genres = styled.span`
  color: var(--color-light-text);
  font-size: 0.875rem;
  font-weight: 500;
`;

const SectionTitle = styled.h2`
  color: var(--color-dark-text);
  font-size: 1.125rem;
  font-weight: 500;
`;

const Overview = styled.p`
  color: var(--color-dark-text);
  font-size: 0.875rem;
  font-weight: 500;
`;

export default MovieDetail;
