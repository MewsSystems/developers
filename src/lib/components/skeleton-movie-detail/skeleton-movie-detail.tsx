import { Container, MovieGrid } from '@pages/movie-detail/movie-detail.styled';
import { BackButton } from '@pages/movie-detail/movie-detail.styled';
import {
  Poster,
  MovieInfo,
  Title,
  MovieDetails,
  DetailItem,
  ReleaseDate,
  Rating,
  Overview,
} from './skeleton-movie-detail.styled';

export const MovieDetailSkeleton = () => {
  return (
    <Container>
      <BackButton />
      <MovieGrid>
        <Poster />
        <MovieInfo>
          <Title />
          <MovieDetails>
            <DetailItem />
            <DetailItem />
            <DetailItem />
          </MovieDetails>
          <ReleaseDate />
          <Rating />
          <Overview />
        </MovieInfo>
      </MovieGrid>
    </Container>
  );
};
