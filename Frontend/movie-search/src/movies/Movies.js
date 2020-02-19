import React, { useState, useEffect } from "react";
import "../App.css";
import Header from "../components/header/Header";
import Movie from "./movieItem/MovieItem";
import { bindActionCreators } from 'redux';
import * as moviesActions from './MoviesActions';
import { connect } from 'react-redux';
import { MoviesListWrapper, SpinnerWrapper, MoviesContainer, SearchWrapper } from './Movies.styles';
import { PacmanLoader } from "react-spinners";
import useDebounce from '../services/hooks/useDebounce';
import RCPagination from 'rc-pagination';
import 'rc-pagination/assets/index.css';

const Movies = ({
                  moviesActions: {
                    fetchMovies,
                  },
                  moviesReducer: {
                    loading,
                    movies: {
                      page,
                      total_results,
                      total_pages,
                      results
                    }
                  }
                }) => {
  debugger;
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm, 500);
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    debouncedSearchTerm && fetchMovies(debouncedSearchTerm, currentPage)

  }, [debouncedSearchTerm, currentPage]);

  const changePage = (page) => setCurrentPage(page);
  const handleSearchTerm = ({ target: { value } }) => {
    setSearchTerm(value);
    if(value === '')
      setCurrentPage(1)
  };

  return (
    <MoviesContainer>
      <Header text="Movies"/>
      <SearchWrapper>
        <input
          placeholder="Search Movies"
          onChange={ handleSearchTerm }
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
          : debouncedSearchTerm && (
          <>
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
            <RCPagination
              //locale={ localeInfo }
              // pageSizeOptions={['5', '10', '20', '50']}
              // selectComponentClass={Select}
              // showSizeChanger={window.screen.width >= 600}
              //showQuickJumper={ window.screen.width >= 600 && { goButton: <button>Open</button> }}
              defaultPageSize={ 20 }
              defaultCurrent={ currentPage }
              // onShowSizeChange={onChangePagination}
              onChange={ changePage }
              total={ total_results }
              showTotal={ (total, range) => `${range[0]} - ${range[1]} of ${total} items` }
            />
          </>
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

