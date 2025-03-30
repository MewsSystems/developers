import styled from 'styled-components';
import { useParams, Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { fetchMovieDetails } from '../search-api.tsx';
import { PageSection } from '../components/PageSection.tsx';
import fallback_image from './../assets/image-load-failed.svg';
import { NotFoundPage } from './NotFoundPage.tsx';

const StyledColumn = styled.div`
  display: flex;
  flex-direction: column;
`;

const StyledMovieInfo = styled(StyledColumn)`
  gap: 2rem;
  max-width: 50%;
  text-align: left;
`;

const StyledMovieDetailSection = styled(StyledColumn)`
  gap: 0.25rem;
`;

const StyledMovieYearOfRelease = styled.span`
  font-weight: 500;
  font-size: clamp(1.75rem, 1.5833rem + 0.8333vw, 2.25rem);
  opacity: 0.6;
`;

const StyledMovieGenreWrapper = styled.ul`
  display: flex;
  flex-direction: row;
  gap: 0.5rem;
`;

const StyledMovieGenre = styled.li`
  background-color: var(--c-neutral-light);
  padding: 0.25rem;
  border-radius: var(--br-rounded);
  list-style: none;
`;

interface Genre {
  id: number;
  name: string;
}

export const MovieDetailPage = () => {
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
        <Link
          className="gradient-hover f-link-md"
          to="/"
          aria-label="Back to search"
        >
          ← Back to search
        </Link>
      </PageSection>
      <PageSection direction="row" $backgroundcolor="#e0e0e0">
        <img
          src={
            movie.poster_path
              ? `https://image.tmdb.org/t/p/w300/${movie.poster_path}`
              : fallback_image
          }
          alt={`Movie poster for ${movie.title}`}
        />
        <StyledMovieInfo>
          <StyledMovieDetailSection>
            <h1>
              {movie.title}{' '}
              <StyledMovieYearOfRelease>
                ({releaseYear})
              </StyledMovieYearOfRelease>
            </h1>
            <StyledMovieGenreWrapper className="f-p2">
              {movie?.genres.map((genre: Genre) => {
                return (
                  <StyledMovieGenre key={genre.name}>
                    {genre.name}
                  </StyledMovieGenre>
                );
              })}
            </StyledMovieGenreWrapper>
          </StyledMovieDetailSection>
          <StyledMovieDetailSection className="f-p2">
            <p>
              <strong>Average rating: </strong>
              {movie.vote_average}
            </p>
            <p>
              <strong>Runtime:</strong> {`${movie.runtime} min`}
            </p>
          </StyledMovieDetailSection>
          <StyledMovieDetailSection>
            <p>
              <em>{movie.tagline}</em>
            </p>
            <h2>Overview</h2>
            <p>{movie.overview}</p>
          </StyledMovieDetailSection>
        </StyledMovieInfo>
      </PageSection>
    </>
  );
};
