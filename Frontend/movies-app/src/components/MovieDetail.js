import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';

import * as actions from './../data/state/actions';
import { Column, SubTitle, Title, Link, Paragraph, GenreList, Genre, FieldLabel, FlexContainer } from './styled';

const MovieDetails = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const item = useSelector(state => state.detail.item);
  useEffect(() => {
    dispatch(actions.LoadDetail(id));
  }, [id, dispatch]);

  return item ? (
    <FlexContainer>
      <Column>
        <Link href={`https://image.tmdb.org/t/p/original${item.poster_path}`}>
          <img
            src={`https://image.tmdb.org/t/p/w500${item.poster_path}`}
            alt={`${item.title}_poster`}
            onError={e => {
              e.target.onerror = null;
              e.target.src = 'no_image.png';
            }}
          />
        </Link>
      </Column>
      <Column>
        <Title>{item.title}</Title>
        {item.tagline && (
          <SubTitle wrap italic>
            {item.tagline}
          </SubTitle>
        )}
        <Paragraph italic>
          <FieldLabel>Overview:</FieldLabel>
          {item.overview}
        </Paragraph>
        <Paragraph>
          <FieldLabel>Release date:</FieldLabel>
          {item.release_date}
        </Paragraph>
        {item.genres && (
          <Paragraph>
            <FieldLabel>Genres:</FieldLabel>
            <GenreList>
              {item.genres.map(g => (
                <Genre key={g.id}>{g.name}</Genre>
              ))}
            </GenreList>
          </Paragraph>
        )}
        {item.homepage && (
          <Paragraph>
            <Link href={item.homepage}>Home page</Link>
          </Paragraph>
        )}
        {item.backdrop_path && (
          <Paragraph>
            <Link href={`https://image.tmdb.org/t/p/original${item.backdrop_path}`}>Backdrop</Link>
          </Paragraph>
        )}
      </Column>
    </FlexContainer>
  ) : (
    <div></div>
  );
};

export default MovieDetails;
