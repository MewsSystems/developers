import React from 'react';
import Moment from 'react-moment';
import { Link } from 'react-router-dom';

const MovieItem = ({ movie }) => {
  return (
    <li className='collection-item'>
      <div>
        <Link to={`/movie/${movie.id}`}>{movie.title}</Link>
        <br />
        <span className='black-text'>
          <span className='grey-text'>Released on</span>{' '}
          <Moment format='MMMM Do YYYY'>{new Date(movie.release_date)}</Moment>
        </span>
        <span href='#!' className='secondary-content'>
          <div className='secondary-content'>
            <span className='grey-text'>Rating: </span>
            <span className='black-text'>{movie.vote_average}</span>
          </div>
        </span>
      </div>
    </li>
  )
};

export default MovieItem;
