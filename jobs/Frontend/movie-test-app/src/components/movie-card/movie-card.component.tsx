import { FC } from 'react';
import { Result } from '../../types/api.ts';
import { Footer, MovieCardContainer, MovieImageContainer, Name } from './movie-card.styles.tsx';
import { useImage } from '../../app/api/images.ts';
import Spinner from '../spinner';

type MovieCardProps = {
  movie: Result;
  handleMovieClick: () => void;
};

const defaultImageSize = 400;

const MovieCard: FC<MovieCardProps> = ({ movie, handleMovieClick }) => {
  const { title } = movie;
  const postCardImage = useImage({ imagePath: movie?.poster_path, imageWidth: defaultImageSize });

  if (postCardImage.isLoading) {
    return <Spinner />;
  }

  if (postCardImage.isError) {
    return <div>Something went wrong</div>;
  }

  return (
    <MovieCardContainer onClick={handleMovieClick}>
      <MovieImageContainer>
        <img src={postCardImage.data} alt={title} />
      </MovieImageContainer>
      <Footer>
        <Name>{title}</Name>
      </Footer>
    </MovieCardContainer>
  );
};

export { MovieCard };
