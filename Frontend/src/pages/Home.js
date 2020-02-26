import React, { Fragment } from 'react';
import { connect } from 'react-redux';
import Search from '../components/movies/Search';
import Movies from '../components/movies/Movies';
import Pagination from '../components/movies/Pagination';
import { searchMovies, setSearchTerm } from '../actions/movieActions';

const Home = ({ searchMovies, setSearchTerm, movie: { searchTerm } }) => {

  const handleSearchChange = query => {
    setSearchTerm(query);
    searchMovies(query);
  };

  const handleClick = number => {
    searchMovies(searchTerm, number);
  };

  return (
    <Fragment>
      <div className="container">
        <Search change={handleSearchChange} />
        <Movies />
        <Pagination click={handleClick} />
      </div>
    </Fragment>
  )
};

const mapStateToProps = state => ({
  movie: state.movie
});

export default connect(mapStateToProps, { searchMovies, setSearchTerm })(Home);
