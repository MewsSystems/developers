import _ from 'lodash';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { GetMovie } from '../actions/movieActions';
import styled from 'styled-components';

const Wrapper = styled.div`
  width: 90vw;
  margin: 0 auto;
  padding-top: 40px;

  @media (min-width: 768px) {
    max-width: 900px;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: space-between;
  }
`;

const StyledDivImg = styled.div`
  margin: 0 auto;
  width: 50%;

  img {
    width: 100%;
    margin: 0 auto;
    border-radius: 15px;
  }

  @media (min-width: 768px) {
    margin: 0;
    width: 20%;
  }
`;

const StyledDivInfo = styled.div`
  h3 {
    font-weight: normal;
    font-size: 24px;
    text-align: center;
    margin-bottom: 8px;
    color: #bd7898;
  }
  p {
    text-align: center;
  }

  @media (min-width: 768px) {
    width: 75%;

    h3,
    p {
      text-align: left;
    }
  }
`;

const MovieDetail = (props) => {
  const movieId = props.match.params.movieId;
  const dispatch = useDispatch();
  const movieState = useSelector((state) => state.Movie);

  useEffect(() => {
    dispatch(GetMovie(movieId));
  }, []);

  const ShowData = () => {
    if (!_.isEmpty(movieState.data[movieId])) {
      const movie = movieState.data[movieId];
      console.log(movieId);

      return (
        <Wrapper>
          <StyledDivImg>
            {movie.poster_path === null ? (
              <img src={`http://placekitten.com/200/300`} alt="movie poster" />
            ) : (
              <img
                src={`http://image.tmdb.org/t/p/w185${movie.poster_path}`}
                alt="movie poster"
              />
            )}
          </StyledDivImg>

          <StyledDivInfo>
            <h3>{movie.title.toUpperCase()}</h3>
            <p>
              {movie.release_date &&
                movie.release_date
                  .substring(5)
                  .split('-')
                  .concat(movie.release_date.substring(0, 4))
                  .join('/')}
            </p>
            <p>{movie.overview}</p>
          </StyledDivInfo>
        </Wrapper>
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
