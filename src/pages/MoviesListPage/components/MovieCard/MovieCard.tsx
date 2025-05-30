import type {Movie} from '../../../../api/types';
import {Card, Content, MetaInfo, Overview, Rating, StyledLink, Title} from './MovieCard.styled';
import MovieCover from '../../../common/MovieCover/MovieCover';
import {PathByPageType} from '../../../../routes/constants';
import {generatePath} from 'react-router-dom';

type MovieCardProps = {
  movie: Movie;
  searchParams: URLSearchParams;
  searchQuery: string;
  currentPage: number;
};

export default function MovieCard({movie, searchParams, searchQuery, currentPage}: MovieCardProps) {
  const {release_date, title, id, vote_average, poster_path, overview} = movie;

  const releaseYear = release_date ? new Date(release_date).getFullYear() : 'N/A';
  const rating = vote_average ? Math.round(vote_average * 10) / 10 : 0;
  const path = generatePath(PathByPageType.MovieDetailsPage, {id: String(id)});
  const movieDetailsPageUrl = `${path}?${String(searchParams)}`;

  return (
    <StyledLink
      to={movieDetailsPageUrl}
      state={{
        from: '/',
        search: searchQuery,
        page: currentPage,
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
