import React from "react";
import { CardWrapper } from './MovieItem.style';
import { withRouter } from 'react-router-dom';

const Movie = ({ movie: { title, id, poster_path, popularity }, history }) => (
  <CardWrapper onClick={ () => history.push(`/${id}`) }>
    <h2>{ title }</h2>
    <div>
      <img
        width="200"
        src={ `https://image.tmdb.org/t/p/w500_and_h282_face/${poster_path}` }
      />
    </div>
    <p>Popularity - { popularity }</p>
  </CardWrapper>
);

export default withRouter(Movie);
