import React, { useState, useEffect } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import URLSearchParams from 'query-string';

import * as actions from './../data/state/actions';
import useDebounce from './../hooks/useDebounce';
import MovieList from './MovieList';
import SearchBox from './SearchBox';
import Pagination from './Pagination';
import { Section, Row, Paragraph } from './styled';

const Home = () => {
  const { search } = useLocation();
  const params = URLSearchParams.parse(search);
  const pageIndex = params['page'] || 1;

  const history = useHistory();

  const dispatch = useDispatch();
  const movies = useSelector(state => state.movies.items);
  const currentPage = useSelector(state => state.movies.currentPage);
  const totalPages = useSelector(state => state.movies.totalPages);
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm, 500);

  useEffect(() => {
    dispatch(actions.LoadMovies(debouncedSearchTerm, pageIndex));
  }, [debouncedSearchTerm, dispatch, pageIndex]);

  const pageChanged = newPage => {
    window.scrollTo(0, 0);
    history.push(`/?page=${newPage}`);
  };

  return (
    <>
      <Section>
        <SearchBox
          searchTerm={searchTerm}
          placeHolder="Search for movies"
          onSearchTermChanged={e => setSearchTerm(e.target.value)}
        />
      </Section>
      <Section>
        <Row align="right">
          <Pagination currentPage={currentPage} onPageChanged={pageChanged} totalPages={totalPages} />
        </Row>
        <MovieList movies={movies} empty={<Paragraph bold>No matches have been found.</Paragraph>} />
        <Row align="right">
          <Pagination currentPage={currentPage} onPageChanged={pageChanged} totalPages={totalPages} />
        </Row>
      </Section>
    </>
  );
};

export default Home;
