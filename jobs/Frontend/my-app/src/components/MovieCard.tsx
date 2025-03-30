import styled from 'styled-components';
import fallback_image from './../assets/image-load-failed.svg';
import { Link } from 'react-router-dom';

const StyledMovieCard = styled.div`
  display: flex;
  flex-flow: column nowrap;
  max-width: 200px;
  width: 100%;
  align-items: center;
  align-self: baseline;
`;

const StyledMovieCardPoster = styled.img`
  max-width: 150px;
  height: 225px;
  width: auto;
  transition: var(--transition-prim);
  &:hover {
    transform: scale3d(1.04, 1.04, 1.04);
  }
`;

interface MovieCardProps {
  poster: string;
  name: string;
  rating: string;
  release_date: string;
  to: string;
}

export const MovieCard = ({
  poster,
  name,
  rating,
  release_date,
  to,
}: MovieCardProps) => {
  const formattedReleaseDate = release_date
    ? new Date(release_date).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      })
    : 'Unknown';

  return (
    <StyledMovieCard>
      <Link to={to}>
        <StyledMovieCardPoster
          src={
            poster
              ? `https://image.tmdb.org/t/p/w200/${poster}`
              : fallback_image
          }
          alt={`Movie poster for ${name}`}
          style={{
            opacity: poster ? 1 : 0.2,
          }}
        />
      </Link>
      <h2 className="f-h3">{name}</h2>
      <div className="f-p2">
        <p>{rating}</p>
        <p>{formattedReleaseDate}</p>
      </div>
    </StyledMovieCard>
  );
};
