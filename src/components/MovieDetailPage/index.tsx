import React, { FC } from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { BackLink, Body, DetailWrapper, Details, Info, InfoItem, Overview, Poster } from './styles';
import { Movie } from '../../types';

import { Link } from 'react-router-dom';

const MovieDetailPage: FC<{ detail?: Movie; dispatch?: Dispatch }> = ({ detail }) => {
  if (!Object.keys(detail).length)
    return (
      <BackLink>
        <Link to="/">Back</Link>
      </BackLink>
    );

  return (
    <DetailWrapper backdrop={`https://image.tmdb.org/t/p/w500/${detail.backdrop_path}`}>
      <h1>{detail.title}</h1>
      <Body>
        <Poster>
          <img src={`https://image.tmdb.org/t/p/w500/${detail.poster_path}`} alt={detail.title} />
        </Poster>
        <Details>
          <Overview>
            <div>Overview:</div>
            <p>{detail.overview}</p>
          </Overview>
          <Info>
            <InfoItem>Release Date: {detail.release_date}</InfoItem>
            <InfoItem>Original Title: {detail.original_title}</InfoItem>
            <InfoItem>Language: {detail.original_language}</InfoItem>
            <InfoItem>Popularity: {detail.popularity}</InfoItem>
            <InfoItem>Vote Average: {detail.vote_average}</InfoItem>
            <InfoItem>Vote Count: {detail.vote_count}</InfoItem>
          </Info>
        </Details>
      </Body>
      <BackLink>
        <Link to="/">Back</Link>
      </BackLink>
    </DetailWrapper>
  );
};

const mapStateToProps = (state: any) => {
  return state;
};

export default connect(mapStateToProps)(MovieDetailPage);
