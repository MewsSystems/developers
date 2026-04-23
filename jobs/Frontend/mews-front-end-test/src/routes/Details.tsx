import React, { FC } from 'react';
import { Header } from '../components/Header/Header';
import { StyledAppContainer } from './Home.styled';
import { getCurrentMoviesSelector } from '../redux/selectors';
import { useAppSelector } from '../redux/hooks/hooks';
import { Movie } from '../api/sendRequest';
import { StyledLink, StyledParagraph } from './Details.styled.';

interface Props {
  movie: Movie;
}

const DetailsDisplay: FC<Props> = ({ movie }) => {
  const releaseDate = new Date(movie.release_date).toLocaleDateString();

  return (
    <StyledAppContainer>
      <Header />
      <StyledLink to={'/'}>Back to search</StyledLink>
      <h2>{movie.title}</h2>

      <section>
        <StyledParagraph>{`Adult content: ${movie.adult ? 'Yes' : 'No'}`}</StyledParagraph>
        <StyledParagraph>{`Movie ID: ${movie.id}`}</StyledParagraph>
        <StyledParagraph>{`Overview: ${movie.overview}`}</StyledParagraph>
        <StyledParagraph>{`Popularity: ${movie.popularity}`}</StyledParagraph>
        <StyledParagraph>{`Release date: ${releaseDate}`}</StyledParagraph>
        <StyledParagraph>{`Average rating: ${movie.vote_average}`}</StyledParagraph>
        <StyledParagraph>{`Vote count: ${movie.vote_count}`}</StyledParagraph>
      </section>
    </StyledAppContainer>
  );
};

const Details: FC = () => {
  const movie = useAppSelector(getCurrentMoviesSelector);

  return <DetailsDisplay movie={movie} />;
};

export { Details, DetailsDisplay };
