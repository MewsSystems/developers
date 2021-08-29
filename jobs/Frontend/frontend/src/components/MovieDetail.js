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

  // const ShowData = () => {
  //   if (!_.isEmpty(movieState.data[movieId])) {
  //     const movieData = movieState.data[movieId];
  //     console.log(movieId);

  //     return (
  //       <div>
  //         <div>{movieData.title}</div>
  //       </div>
  //     );
  //   }

  //   if (movieState.loading) {
  //     <p>loading</p>;
  //   }

  //   if (movieState.errorMsg !== '') {
  //     return <p>{movieState.errorMsg}</p>;
  //   }
  // };

  return <h1>Detail</h1>;
};

export default MovieDetail;
