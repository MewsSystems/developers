import MovieList from '../components/MovieList';
import Pagination from '../components/Pagination';
import SearchArea from '../components/SearchArea';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { GetMovieList } from '../actions/movieActions';
import useDebounce from '../hook/useDebounce';
import { GetAllMovie } from '../actions/movieActions';
import styled from 'styled-components';

const Wrapper = styled.div`
  width: 80%;
  margin: 0 auto;

  @media (min-width: 1000px) {
    max-width: 1000px;
  }
`;

const HomePage = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const debouncedSearchTerm = useDebounce(searchTerm, 500);

  const dispatch = useDispatch();

  const allMovie = useSelector((state) => state.AllMovie);
  const movieList = useSelector((state) => state.MovieList);

  const FetchData = (searchTerm = debouncedSearchTerm) => {
    dispatch(GetAllMovie(searchTerm));
  };

  const OnePage = (pageNumber = 1) => {
    dispatch(GetMovieList(pageNumber, searchTerm));
    setCurrentPage(pageNumber);
  };

  useEffect(() => {
    if (debouncedSearchTerm) {
      FetchData(debouncedSearchTerm);
      OnePage();
    }
  }, [debouncedSearchTerm]);

  const handleChange = (e) => {
    setSearchTerm(e.target.value);
  };

  return (
    <>
      <SearchArea handleChange={handleChange} />
      <Wrapper>
        <MovieList movieList={movieList} />
        {allMovie.pages > 1 ? (
          <Pagination
            pages={allMovie.pages}
            nextPage={OnePage}
            currentPage={currentPage}
          />
        ) : (
          ''
        )}
      </Wrapper>
    </>
  );
};

export default HomePage;
