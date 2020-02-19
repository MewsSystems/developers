import React from "react";
import { CardWrapper } from './MovieItem.style';

const Movie = ({ movie: { title, id, poster_path, popularity } }) => (
  <CardWrapper>
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

export default Movie;
