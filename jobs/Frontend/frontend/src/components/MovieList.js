import React from 'react';
import _ from 'lodash';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

const Wrapper = styled.div`
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: space-between;
`;

const StyledDiv = styled.div`
  width: 45%;
  margin-bottom: 10px;

  @media (min-width: 768px) {
    width: 18%;
  }
`;

const StyledImg = styled.img`
  width: 100%;
  border-radius: 15px;
  margin-top: 10px;
  margin-bottom: 5px;
  box-shadow: 2px 2px 10px #222121af;
`;

const StyledDivInfo = styled.div`
  margin-left: 10%;
  font-size: 14px;
  width: 85%;

  a {
    font-weight: 600;
  }

  a:hover {
    color: #bd7898;
  }

  p {
    margin: 5px 0;
    color: gray;
  }
`;

const MovieList = ({ movieList }) => {
  const ShowData = () => {
    if (!_.isEmpty(movieList.data)) {
      return (
        <Wrapper>
          {movieList.data.map((movie) => {
            return (
              <StyledDiv key={movie.id}>
                {movie.poster_path === null ? (
                  <StyledImg
                    src={`http://placekitten.com/200/300`}
                    alt="movie poster"
                  />
                ) : (
                  <StyledImg
                    src={`http://image.tmdb.org/t/p/w185${movie.poster_path}`}
                    alt="movie poster"
                  />
                )}

                <StyledDivInfo>
                  <Link to={`/movie/${movie.id}`}>{movie.title}</Link>
                  <p> {movie.release_date && movie.release_date.slice(0, 4)}</p>
                </StyledDivInfo>
              </StyledDiv>
            );
          })}
        </Wrapper>
      );
    }

    if (movieList.loading) {
      return <p>Loading ...</p>;
    }
  };

  return <div>{ShowData()}</div>;
};

export default MovieList;
