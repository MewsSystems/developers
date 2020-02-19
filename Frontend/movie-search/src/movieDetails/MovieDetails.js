import React, { useState, useEffect } from "react";
import "../App.css";
import Header from "../components/header/Header";
import { bindActionCreators } from 'redux';
import * as movieDetailsActions from './MovieDetailsActions';
import { connect } from 'react-redux';
import {
  MoviesDetailsWrapper,
  SpinnerWrapper,
  MoviesDetailsContainer,
  MovieDetails,
  Overview,
  Company,
  Companies
} from './MovieDetails.styles';
import { PacmanLoader } from "react-spinners";

const Movies = ({
                  match: { params: { movieId } },
                  moviesActions: {
                    fetchMovieDetails,
                  },
                  movieDetailsReducer: {
                    loading,
                    movie
                  }
                }) => {


  useEffect(() => {
    movieId && fetchMovieDetails(movieId)

  }, [movieId]);

  const { title, overview, popularity, poster_path, tagline, vote_average, production_companies } = movie || {};
  return (
    <MoviesDetailsWrapper>
      <Header text="Movies"/>
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
            <>
              <MoviesDetailsContainer>
                <img
                  style={ { flex: 1 } }
                  width="400"
                  src={ `https://image.tmdb.org/t/p/w500_and_h282_face/${poster_path}` }
                />
                <MovieDetails>
                  <p>{ tagline }</p>
                  <h2>{ title }</h2>
                  <p>Popularity - { popularity }</p>
                  <p>Vote - { vote_average }</p>
                  <Overview>{ overview }</Overview>
                  {
                    !!production_companies &&
                      <>
                        <h3>Companies: </h3>
                        <Companies>
                          {
                            !!production_companies && production_companies.map(company => <Company
                              key={ company.id }>{ company.name }</Company>)
                          }
                        </Companies>
                      </>
                  }


                </MovieDetails>
              </MoviesDetailsContainer>
            </>
          )
      }
    </MoviesDetailsWrapper>
  );
};

function mapStateToProps(state) {
  return {
    movieDetailsReducer: state.movieDetailsReducer,
  };
}

function mapDispatchToProps(dispatch) {
  return {
    moviesActions: bindActionCreators(movieDetailsActions, dispatch),
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Movies);

