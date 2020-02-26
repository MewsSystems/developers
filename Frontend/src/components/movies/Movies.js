import React, { Fragment } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Preloader from '../layout/Preloader';
import MovieItem from './MovieItem';

const Movies = ({ movie: { movies, loading } }) => {

  if (loading || movies === null) {
    return <Preloader />
  }

  return (
    <Fragment>
      <ul className="collection with-header">
        <li className="collection-header">
          <h4 className="center">Movies</h4>
        </li>
        {!loading && (movies.length === 0) ? (
            <p className="center">No movies to show. Do a search!</p>
          ) :
          (
            movies.map(movie => <MovieItem key={movie.id} movie={movie} />)
          )}
      </ul>
    </Fragment>
  )
};

Movies.propTypes = {
  movie: PropTypes.object.isRequired
};

const mapStateToProps = state => ({
  movie: state.movie
});

export default connect(mapStateToProps, null)(Movies);
