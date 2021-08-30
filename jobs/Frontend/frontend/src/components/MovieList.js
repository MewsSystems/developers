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
      return (
        <div className="container_movies">
          {movieList.data.map((movie) => {
            return (
              <div className="container_movie" key={movie.id}>
                <div className="movie_poster">
                  {movie.poster_path === null ? (
                    <img
                      src={`http://placekitten.com/200/300`}
                      alt="movie poster"
                    />
                  ) : (
                    <img
                      src={`http://image.tmdb.org/t/p/w185${movie.poster_path}`}
                      alt="movie poster"
                    />
                  )}
                </div>
                <Link to={`/movie/${movie.id}`}>{movie.title}</Link>
                <p> {movie.release_date && movie.release_date.slice(0, 4)}</p>
              </div>
            );
          })}
        </div>
      );
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
