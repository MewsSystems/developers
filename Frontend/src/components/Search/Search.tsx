import React, { useEffect } from 'react';
import { useDispatch, useSelector, shallowEqual } from 'react-redux';
import ReactPaginate from 'react-paginate';

import scrollToTop from '../../utils/scrollToTop';
import { RootReducer } from '../../reducers';
import { fetchTopRatedMovies, searchMovies, handleSearchChange } from '../../actions/search';
import config from '../../config';

import { Title, SearchResultsWrapper, SearchInput, MovieLink, MoviePosterImage } from './Styled';

const Search = () => {
  const dispatch = useDispatch();
  const loading = useSelector((state: RootReducer) => state.search.loading);
  const searchValue = useSelector((state: RootReducer) => state.search.value);
  const list = useSelector((state: RootReducer) => state.search.list);
  const pagination = useSelector((state: RootReducer) => state.search.pagination, shallowEqual);

  useEffect(() => {
    // there is a warning is browser console about this, but this is OK
    // I don't want to rerender on every loading change
    // it just controls first fetch and does not fetch when we come back from movie page
    if (loading) {
      dispatch(fetchTopRatedMovies());
    }
  }, [dispatch]);

  const onSearchChange = (event: React.ChangeEvent) => {
    event.preventDefault();
    const value = (event.target as HTMLInputElement).value;
    dispatch(handleSearchChange(value));
    if (value) {
      dispatch(searchMovies(value));
    } else {
      dispatch(fetchTopRatedMovies());
    }
  };

  const handlePageChange = ({ selected }) => {
    if (searchValue) {
      dispatch(searchMovies(searchValue, selected + 1));
    } else {
      dispatch(fetchTopRatedMovies(selected + 1));
    }
    scrollToTop();
  };

  return (
    <div>
      <SearchInput onChange={onSearchChange} placeholder="Type to search..." type="search" value={searchValue} />
      {!searchValue && <Title>Top Rated Movies</Title>}
      <SearchResultsWrapper>
        {list.map((it) => (
          <MovieLink key={it.id} to={`/movie/${it.id}`}>
            {it.poster_path ? (
              <MoviePosterImage src={`${config.IMAGE_BASE_URL}${it.poster_path}`} alt={it.title} title={it.title} />
            ) : (
              <div>{it.title}</div>
            )}
          </MovieLink>
        ))}
      </SearchResultsWrapper>

      <ReactPaginate
        containerClassName="Search-Page-Pagination"
        disableInitialCallback
        forcePage={pagination.page - 1}
        initialPage={0}
        marginPagesDisplayed={2}
        onPageChange={handlePageChange}
        pageCount={pagination.total}
        pageRangeDisplayed={5}
      />
    </div>
  );
};

export default Search;
