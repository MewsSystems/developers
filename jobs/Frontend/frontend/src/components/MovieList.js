import React, { useEffect, useState } from 'react';
import SearchArea from './SearchArea';
import { useDispatch, useSelector } from 'react-redux';
import _ from 'lodash';
import { GetMovieList } from '../actions/movieActions';
import { Link } from 'react-router-dom';
import useDebounce from '../hook/useDebounce';

const MovieList = () => {
  const dispatch = useDispatch();
  const movieList = useSelector((state) => state.MovieList);

  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm, 500);

  const handleChange = (e) => {
    setSearchTerm(e.target.value);
  };

  const FetchData = (searchTerm = debouncedSearchTerm) => {
    dispatch(GetMovieList(searchTerm));
  };

  useEffect(
    () => {
      if (debouncedSearchTerm) {
        FetchData(debouncedSearchTerm);
      }
    },
    [debouncedSearchTerm], // Only call effect if debounced search term changes
  );

  const ShowData = () => {
    if (!_.isEmpty(movieList.data)) {
      return movieList.data.map((movie) => {
        return (
          <div key={movie.id}>
            <p>{movie.title}</p>
            <Link to={`/movie/${movie.id}`}>View</Link>
          </div>
        );
      });
    }
    if (movieList.loading) {
      return <p>Loading ...</p>;
    }

    // if (movieList.errorMsg !== '') {
    //   return <p>{movieList.errorMsg}</p>;
    // }

    // return <p>unable to get data</p>;
  };

  return (
    <>
      <SearchArea handleChange={handleChange} />
      <div>{ShowData()}</div>
    </>
  );
};

export default MovieList;
