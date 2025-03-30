import styled from 'styled-components';
import { useParams, Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { fetchMovieDetails } from '../search-api.tsx';
import { PageSection } from '../components/PageSection';
import fallback_image from './../assets/image-load-failed.svg';
import { NotFoundPage } from './NotFoundPage';

const StyledLink = styled(Link)`
  --primary-color: #141414;

  margin-inline: 0.5rem;
  position: relative;
  display: inline-block;
  font-weight: 700;
  font-size: 1.125rem;
  color: var(--primary-color);
  &:hover,
  &:focus {
    color: var(--primary-color);
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

const StyledMovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: 2rem;
  max-width: 50%;
  text-align: left;
`;

const StyledMovieInfoHeader = styled.div`
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
`;

const StyledMovieTitle = styled.h1`
  --f-h1: clamp(2rem, 1.6rem + 2vw, 3.2rem);
  font-size: var(--f-h1);
  line-height: calc(var(--f-h1) * 1.2);
`;

const StyledMovieData = styled(StyledMovieInfoHeader)`
  font-size: 0.8125rem;
`;

const StyledMovieYearOfRelease = styled.span`
  font-weight: 500;
  font-size: clamp(1.75rem, 1.5833rem + 0.8333vw, 2.25rem);
  opacity: 0.6;
`;

const StyledMovieGenreWrapper = styled.div`
  display: flex;
  flex-direction: row;
  gap: 0.5rem;
  font-size: 0.875rem;
`;

const StyledMovieGenre = styled.span`
  background-color: white;
  padding: 0.25rem;
  border-radius: 10px;
`;

const StyledMovieCopy = styled(StyledMovieInfoHeader)``;

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
  if (isError) return <NotFoundPage />;

  const releaseYear = movie?.release_date
    ? new Date(movie.release_date).getFullYear()
    : 'Unknown';

  return (
    <>
      <PageSection>
        <StyledLink to="/" aria-label="Back to search">
          ← Back to search
        </StyledLink>
      </PageSection>
      <PageSection direction="row" $backgroundcolor="#e0e0e0">
        <StyledMoviePoster
          src={
            movie.poster_path
              ? `https://image.tmdb.org/t/p/w300/${movie.poster_path}`
              : fallback_image
          }
          alt={`Movie poster for ${movie.title}`}
        />
        <StyledMovieInfo>
          <StyledMovieInfoHeader>
            <StyledMovieTitle>
              {movie.title}{' '}
              <StyledMovieYearOfRelease>
                ({releaseYear})
              </StyledMovieYearOfRelease>
            </StyledMovieTitle>
            <StyledMovieGenreWrapper>
              {movie?.genres.map((genre: Genre) => {
                return (
                  <StyledMovieGenre key={genre.name}>
                    {genre.name}
                  </StyledMovieGenre>
                );
              })}
            </StyledMovieGenreWrapper>
          </StyledMovieInfoHeader>
          <StyledMovieData>
            <p>
              <strong>Average rating: </strong>
              {movie.vote_average}
            </p>
            <p>
              <strong>Runtime:</strong> {`${movie.runtime} min`}
            </p>
          </StyledMovieData>
          <StyledMovieCopy>
            <p>
              <em>{movie.tagline}</em>
            </p>
            <h2>Overview</h2>
            <p>{movie.overview}</p>
          </StyledMovieCopy>
        </StyledMovieInfo>
      </PageSection>
    </>
  );
};
