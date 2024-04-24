import React, { FC, useEffect } from 'react';
import { Header } from '../components/Header/Header';
import { StyledAppContainer } from './App.styled';
import { getCurrentMoviesSelector } from '../redux/selectors';
import { Link } from 'react-router-dom';
import { useAppSelector } from '../redux/hooks/hooks';

const Details: FC = () => {
  const movie = useAppSelector(getCurrentMoviesSelector);

  useEffect(() => {
    console.log('movie: ', movie);
  }, []);

  return (
    <StyledAppContainer>
      <Header />
      <Link to={'/'}>Back to search</Link>
      <div>Just a load of stuff about your film</div>

      <section>
        <p>{movie.adult}</p>
        <p>{movie.id}</p>
        <p>{movie.overview}</p>
        <p>{movie.popularity}</p>
        <p>{movie.release_date}</p>
        <p>{movie.vote_average}</p>
        <p>{movie.vote_count}</p>
        <img src={movie.poster_path} alt={'Movie poster'} />
      </section>
    </StyledAppContainer>
  );
};

export { Details };
