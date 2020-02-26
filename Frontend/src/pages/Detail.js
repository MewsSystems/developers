import React, { Fragment, useEffect } from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { getMovie } from '../actions/movieActions';

const Detail = ({ match, getMovie, movie: { selectedMovie, loading } }) => {

  useEffect(() => {
    getMovie(match.params.id);
    //eslint-disable-next-line
  }, []);

  if (loading || selectedMovie === null) {
    return <h4>Loading...</h4>;
  }

  return (
    <Fragment>
      <Link to={'/'} className="btn waves-effect waves-light grey" style={{ margin: '10px' }}>Go back
        <i className="material-icons left">arrow_back</i>
      </Link>
      <div className="container">
        <h5>{ selectedMovie.title }</h5>
        <div className="divider" />
        <div className="section">
          <div className="row">
            <div className="col s6">
              <img className="z-depth-4" alt={selectedMovie.title} src={`https://image.tmdb.org/t/p/w300/${selectedMovie.poster_path}`} />
            </div>
            <div className="col s6">
              <p>{selectedMovie.overview}</p>
              <div>
                <strong>Language: </strong><span>{selectedMovie.original_language}</span><br />
                <strong>Release date: </strong><span>{selectedMovie.release_date}</span><br />
                <strong>Genre(s): </strong>
                { selectedMovie.genres.map(genre => <span key={genre.id}>{genre.name} </span>) }
                <br />
                <a href={selectedMovie.homepage}>Website</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Fragment>
  )
};

const mapStateToProps = state => ({
  movie: state.movie
});

export default connect(mapStateToProps, { getMovie })(Detail);
