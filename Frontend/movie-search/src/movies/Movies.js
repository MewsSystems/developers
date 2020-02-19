import React, { useState, useEffect } from "react";
import "../App.css";
import Header from "../components/header/Header";
import Movie from "./movieItem/MovieItem";
import { bindActionCreators } from 'redux';
import * as moviesActions from './MoviesActions';
import { connect } from 'react-redux';
import { MoviesListWrapper, InfoFormWrapper, SpinnerWrapper, MoviesContainer, SearchWrapper } from './Movies.styles';
import { PacmanLoader } from "react-spinners";
import useDebounce from '../services/hooks/useDebounce';

const Movies = ({
                  moviesActions: {
                    fetchMovies,
                  },
                  moviesReducer: {
                    loading,
                    movies: {
                      page,
                      totalResults,
                      totalPages,
                      results
                    }
                  }
                }) => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm, 500);

  useEffect(() => {
    debouncedSearchTerm && fetchMovies(debouncedSearchTerm)

  }, [debouncedSearchTerm]);

  return (
    <MoviesContainer>
      <Header text="Movies"/>
      <SearchWrapper>
        <input
          placeholder="Search Movies"
          onChange={ e => setSearchTerm(e.target.value) }
        />
      </SearchWrapper>

      <div>Start searching to display movies</div>
      {
        loading
          ? (
            <SpinnerWrapper>
              <PacmanLoader
                size={ 50 }
                color={ "#123abc" }
                loading={ loading }
              />
            </SpinnerWrapper>
          )
          : (
            <MoviesListWrapper>
              {
                results.map((movie) => (
                  <Movie
                    key={ movie.id }
                    movie={ movie }
                  />
                ))
              }
            </MoviesListWrapper>
          )
      }
    </MoviesContainer>
  );
};

function mapStateToProps(state) {
  return {
    moviesReducer: state.moviesReducer,
  };
}

function mapDispatchToProps(dispatch) {
  return {
    moviesActions: bindActionCreators(moviesActions, dispatch),
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Movies);

