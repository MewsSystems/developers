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
  const [currentPage, setCurrentPage] = useState(null);
  const debouncedSearchTerm = useDebounce(searchTerm, 500);

  const dispatch = useDispatch();

  const allMovie = useSelector((state) => state.AllMovie);
  const movieList = useSelector((state) => state.MovieList);

  // when returning from movieDetail, the previous state is set
  useEffect(() => {
    let storePage = Number(sessionStorage.getItem('page'));

    if (currentPage === null && storePage === 0) {
      setCurrentPage(null);
    } else {
      setCurrentPage(storePage);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (debouncedSearchTerm) {
      FetchData(debouncedSearchTerm);
      OnePage();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [debouncedSearchTerm]);

  const FetchData = () => {
    dispatch(GetAllMovie(debouncedSearchTerm));
  };

  // finds a new movieList based on the current page and search term
  const OnePage = (page) => {
    let search = sessionStorage.getItem('search');
    let storePage = Number(sessionStorage.getItem('page'));

    if (searchTerm === '') {
      setSearchTerm(search);
    }

    let newPage;

    if (page) {
      newPage = page;
    } else if (storePage !== 0) {
      newPage = storePage;
    } else {
      newPage = 1;
    }

    dispatch(GetMovieList(newPage, searchTerm === '' ? search : searchTerm));
    setCurrentPage(newPage);
    sessionStorage.setItem('page', newPage);
  };

  const handleChange = (e) => {
    setSearchTerm(e.target.value);
    if (e.target.value.length === 1) {
      sessionStorage.clear();
    }
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
