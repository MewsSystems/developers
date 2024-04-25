import React, { FC } from 'react';
import { Header } from '../components/Header/Header';
import { StyledAppContainer } from './App.styled';
import { getCurrentMoviesSelector } from '../redux/selectors';
import { Link } from 'react-router-dom';
import { useAppSelector } from '../redux/hooks/hooks';
import { Movie } from '../api/sendRequest';

interface Props {
  movie: Movie;
}

const DetailsDisplay: FC<Props> = ({ movie }) => {
  return (
    <StyledAppContainer>
      <Header />
      <Link to={'/'}>Back to search</Link>
      <div>Just a load of stuff about your film</div>

      <section>
        <p>{`Adult content: ${movie.adult ? 'Yes' : 'No'}`}</p>
        <p>{`Movie ID: ${movie.id}`}</p>
        <p>{`Overview: ${movie.overview}`}</p>
        <p>{`Popularity: ${movie.popularity}`}</p>
        <p>{`Release date: ${movie.release_date}`}</p>
        <p>{`Average rating: ${movie.vote_average}`}</p>
        <p>{`Vote count: ${movie.vote_count}`}</p>
      </section>
    </StyledAppContainer>
  );
};

const Details: FC = () => {
  const movie = useAppSelector(getCurrentMoviesSelector);

  return <DetailsDisplay movie={movie} />;
};

export { Details, DetailsDisplay };
