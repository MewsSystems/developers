import _ from 'lodash';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { GetMovie } from '../actions/movieActions';

const MovieDetail = (props) => {
  const movieId = props.match.params.movieId;
  const dispatch = useDispatch();
  const movieState = useSelector((state) => state.Movie);

  useEffect(() => {
    dispatch(GetMovie(movieId));
  }, []);

  console.log(movieState.data);
  // console.log(movieState.data.undefined.title);

  const ShowData = () => {
    if (!_.isEmpty(movieState.data[movieId])) {
      const movie = movieState.data[movieId];
      console.log(movieId);

      return (
        <div className="container_detail">
          <div className="detail_poster">
            {movie.poster_path === null ? (
              <img src={`http://placekitten.com/200/300`} alt="movie poster" />
            ) : (
              <img
                src={`http://image.tmdb.org/t/p/w185${movie.poster_path}`}
                alt="movie poster"
              />
            )}
          </div>
          <div>
            <div className="detail_info">
              <div className="detail_header">
                <h3>{movie.title.toUpperCase()}</h3>
                <p className="detail_date">
                  {movie.release_date &&
                    movie.release_date
                      .substring(5)
                      .split('-')
                      .concat(movie.release_date.substring(0, 4))
                      .join('/')}
                </p>
              </div>
              <p>{movie.overview}</p>
            </div>
          </div>
        </div>
      );
    }

    if (movieState.loading) {
      <p>loading</p>;
    }

    if (movieState.errorMsg !== '') {
      return <p>{movieState.errorMsg}</p>;
    }
  };

  return (
    <>
      <div>{ShowData()}</div>
    </>
  );
};

export default MovieDetail;
