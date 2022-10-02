import React, { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import styled from 'styled-components';
import Header from '../components/detail/DetailHeader';
import { Container } from '../styles/Container.styled';
import {
  DETAIL_ENDPOINT, API_AUTH, PAGE_TITLE, isoLangs, getReleaseYear,
} from '../utils';

const BackButton = styled(Link)`
  background: #212121;
  border-radius: 4px;
  color: #fff;
  display: block;
  font-weight: 700;
  margin: 60px auto;
  max-width: 240px;
  padding: 16px;
  text-align: center;
  text-decoration: none;
  transition: background .2s;
  
  &:hover,
  &:active,
  &:focus {
    background: #000;
  }
`;

const Overview = styled.p`
  font-size: 20px;
  line-height: 1.6;
  margin-bottom: 40px;
`;

const MovieDetails = styled.ul`
  max-width: 600px;
  
  li {
    padding: 8px 2px;
    border-bottom: 1px solid #eee;
  }
`;

function Detail() {
  const [movie, setMovie] = useState<IFetchMovieDetail | false>(false);
  const { movieId } = useParams();

  useEffect(() => {
    fetch(`${DETAIL_ENDPOINT}/${movieId}?${API_AUTH}`)
      .then((response) => response.json())
      .then((data: IFetchMovieDetail) => {
        setMovie(data);
        document.title = `${data.title} - ${PAGE_TITLE}`;
      });

    return function cleanup() {
      document.title = PAGE_TITLE;
    };
  }, []);

  if (movie) {
    return (
      <>
        <Header movie={movie} />
        <Container>
          <Overview>{movie.overview}</Overview>
          <MovieDetails>
            <li>
              {`User rating:
                ${movie.vote_count > 0
                ? `${movie.vote_average.toFixed(1)} / 10`
                : 'No votes yet'}
              `}
            </li>

            {movie.original_title !== movie.title
              && (
                <li>
                  {`Original title: ${movie.original_title}`}
                </li>
              )}

            <li>
              {`Release date: ${movie.release_date ? getReleaseYear(movie.release_date) : 'TBA'}`}
            </li>

            <li>
              {`Language: ${isoLangs[movie.original_language]}`}
            </li>

            {movie.genres.length > 0
              && (
                <li>
                  {`Genres: ${movie.genres.map((genre) => genre.name).join(', ')}`}
                </li>
              )}

            {movie.budget > 0
              && (
                <li>
                  {`Budget: ${movie.budget.toLocaleString()}`}
                </li>
              )}

            {movie.revenue > 0
              && (
                <li>
                  {`Revenue: ${movie.revenue.toLocaleString()}`}
                </li>
              )}

          </MovieDetails>
          <BackButton to="/">Back</BackButton>
        </Container>
      </>
    );
  }
  return (<Container>Loading...</Container>);
}

export default Detail;
