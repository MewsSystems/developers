import styled from 'styled-components';
import { useParams, Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { fetchMovieDetails } from '../search-api';
import { PageSection } from '../components/PageSection';
import fallback_image from './../assets/image-load-failed.svg';

const StyledLink = styled(Link)`
  margin-left: 0.5rem;
  margin-right: 0.5rem;
  position: relative;
  display: inline-block;
  font-weight: 700;
  font-size: 1.125rem;
  color: #141414;
  &:hover,
  &:focus {
    color: #141414;
  }

  &::before {
    content: '';
    position: absolute;
    left: 0;
    bottom: 0;
    height: 3px;
    width: 0;
    background: linear-gradient(to right, #f43f5e, #c026d3, #4d5b9e);
    transition: all 0.3s ease-in-out;
  }

  &:hover::before {
    width: 100%;
  }
`;

const StyledMoviePoster = styled.img`
  border-radius: 10px;
`;

interface Genre {
  id: number;
  name: string;
}

export const MovieDetail = () => {
  const { movieId } = useParams<{ movieId: string }>();
  const numericMovieId = Number(movieId);

  const {
    data: movie,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ['movieDetails', numericMovieId],
    queryFn: () => fetchMovieDetails(numericMovieId),
    enabled: !!movieId,
  });

  if (isLoading) return <div>Loading movie details...</div>;
  if (isError) return <div>Error loading movie details!</div>;

  const releaseYear = movie?.release_date
    ? new Date(movie.release_date).getFullYear()
    : 'Unknown';

  return (
    <>
      <PageSection>
        <StyledLink to="/">← Back to search</StyledLink>
      </PageSection>
      <PageSection direction="row" backgroundColor="#4d5b9e">
        <StyledMoviePoster
          src={
            movie.poster_path
              ? `https://image.tmdb.org/t/p/w300/${movie.poster_path}`
              : fallback_image
          }
          alt="Movie poster"
        />
        <div>
          <h1>
            {movie.title} ({releaseYear})
          </h1>
          <p>
            {movie?.genres.map((genre: Genre) => {
              return <span key={genre.name}>{genre.name}</span>;
            })}
          </p>
        </div>
        <div>
          <p>{movie.vote_average}</p>
          <p>{`${movie.runtime} min`}</p>
        </div>
        <div>
          <p>{movie.tagline}</p>
          <h2>Overview</h2>
          <p>{movie.overview}</p>
        </div>
      </PageSection>
    </>
  );
};
