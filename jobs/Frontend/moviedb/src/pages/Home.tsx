import React from 'react';
import { useSelector } from 'react-redux';
import { RootState } from '../redux/store';
import MovieList from '../components/home/MovieList';
import Header from '../components/home/HomeHeader';
import Pagination from '../components/home/Pagination';
import { Container } from '../styles/Container.styled';

function Home() {
  const { data } = useSelector((state: RootState) => state.search);

  return (
    <>
      <Header />
      <Container>
        <MovieList />
        {data
          && <Pagination />}
      </Container>
    </>
  );
}

export default Home;
