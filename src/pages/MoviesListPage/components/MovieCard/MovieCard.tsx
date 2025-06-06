import {generatePath, useLocation} from 'react-router-dom';
import type {Movie} from '../../../../api/movieApi/types';
import {PathByPageType} from '../../../../routes/constants';
import MovieCover from '../../../common/MovieCover/MovieCover';
import {Card, Content, MetaInfo, Overview, Rating, StyledLink, Title} from './styled';

type MovieCardProps = {
  movie: Movie;
};

export default function MovieCard({movie}: MovieCardProps) {
  const {release_date, title, id, vote_average, poster_path, overview} = movie;
  const location = useLocation();

  const releaseYear = release_date ? new Date(release_date).getFullYear() : 'N/A';
  const rating = vote_average ? Math.round(vote_average * 10) / 10 : 0;

  const movieDetailsPageUrl = generatePath(PathByPageType.MovieDetailsPage, {
    id: String(id),
  });

  return (
    <StyledLink
      to={movieDetailsPageUrl}
      state={{
        previousPath: location.pathname + location.search,
      }}
    >
      <Card>
        <MovieCover poster_path={poster_path} title={title} isMinimized />
        <Content>
          <Title>{title}</Title>
          <Overview>{overview}</Overview>
          <MetaInfo>
            <span>{releaseYear}</span>
            <Rating $rating={rating}>★ {rating}</Rating>
          </MetaInfo>
        </Content>
      </Card>
    </StyledLink>
  );
}

MovieCard.displayName = 'MovieCard';
