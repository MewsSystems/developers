import { FC } from 'react';
import { Result } from '../../types/api.ts';
import { Footer, MovieCardContainer, MovieImageContainer, Name } from './movie-card.styles.tsx';
import { useImage } from '../../app/api/images.ts';
import { useNavigate } from 'react-router-dom';

type MovieCardProps = {
  movie: Result;
};

const defaultImageSize = 300;

const MovieCard: FC<MovieCardProps> = ({ movie }) => {
  const { title } = movie;
  const postCardImage = useImage({ imagePath: movie?.poster_path, imageWidth: defaultImageSize });
  const navigate = useNavigate();

  const handleMovieClick = () => {
    navigate(`/movies/${movie.id}`);
  };

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
