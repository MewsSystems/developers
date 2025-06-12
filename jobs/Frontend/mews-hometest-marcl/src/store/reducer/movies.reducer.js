import {
    SEARCH_MOVIES_LOADING,
    SEARCH_MOVIES_SUCCESS,
    SEARCH_MOVIES_FAILURE,
    GET_MOVIE_DETAILS_LOADING,
    GET_MOVIE_DETAILS_SUCCESS,
    GET_MOVIE_DETAILS_FAILURE,
    SET_SEARCH_TEXT,
    ADD_TO_FAVORITES,
    REMOVE_FROM_FAVORITES 
  } from '../actions/movies.actions';
  
  const initialState = {
    loading: false,
    movies: [],
    favorites: JSON.parse(localStorage.getItem('favorites') || '[]'), //I will store favs into localStorage
    movieDetails: null,
    error: null,
    searchText: '',
    total_pages: 0,
  };

const moviesReducer = (state = initialState, action) => {
    switch (action.type) {
        case SEARCH_MOVIES_LOADING: {
            return {
                ...state,
                loading: true,
                error: null,
            };
        }
        case SEARCH_MOVIES_SUCCESS: {
            const newMovies = action.data.results;

            const uniqueNewMovies = newMovies.filter((newMovie) =>
                !state.movies || !state.movies.some((movie) => movie.id === newMovie.id)
            );

            // Check if action.data.page is 1 (user adding more text) or > 1 (user is scrolling)
            if (action.data.page === 1) {
                // Reset the movies array when adding more text
                return {
                    ...state,
                    loading: false,
                    movies: uniqueNewMovies,
                    total_pages: action.total_pages,
                };
            } else {
                // Append new content when scrolling
                const updatedMovies = state.movies ? [...state.movies, ...uniqueNewMovies] : uniqueNewMovies;
                return {
                    ...state,
                    loading: false,
                    movies: updatedMovies,
                    total_pages: action.total_pages,
                };
            }
        }
        case SEARCH_MOVIES_FAILURE: {
            let details = "";
            if (action.payload.details && action.payload.details.message) {
                details = action.payload.details.message;
            } else if (Array.isArray(action.payload.details)) {
                details = JSON.stringify(action.payload.details, null, 2);
            } else if (action.payload.details) {
                details = action.payload.details.toString();
            } else {
                details = 'Unknown error occurred';
            }
            
            return {
                ...state,
                loading: false,
                movies: null,
                error: {
                    title: action.title,
                    details: details
                },
            };
        }
        case GET_MOVIE_DETAILS_LOADING: {
            return {
                ...state,
                loading: true,
                movieDetails: null,
                error: null,
            };
        }
        case GET_MOVIE_DETAILS_SUCCESS: {
            return {
                ...state,
                loading: false,
                movieDetails: action.data,
            };
        }
        case GET_MOVIE_DETAILS_FAILURE: {
            let details = "";
            if (action.payload.details && action.payload.details.message) {
                details = action.payload.details.message;
            } else if (Array.isArray(action.payload.details)) {
                details = JSON.stringify(action.payload.details, null, 2);
            } else if (action.payload.details) {
                details = action.payload.details.toString();
            } else {
                details = 'Unknown error occurred';
            }
            
            return {
                ...state,
                loading: false,
                movieDetails: null,
                error: {
                    title: action.title,
                    details: details
                },
            };
        }
        case SET_SEARCH_TEXT: {
            return {
              ...state,
              searchText: action.searchText,
            };
        }
        case ADD_TO_FAVORITES: {
            const updatedFavorites = [...state.favorites, action.payload];
            localStorage.setItem('favorites', JSON.stringify(updatedFavorites));
            return {
                ...state,
                favorites: [...state.favorites, action.payload],
            };
        }
        case REMOVE_FROM_FAVORITES: {
            const filteredFavorites = state.favorites.filter(movie => movie.id !== action.payload.id);
            localStorage.setItem('favorites', JSON.stringify(filteredFavorites));

            return {
                ...state,
                favorites: filteredFavorites 
            };
        }
        default:
            return {
                ...state,
            };
    }
}

export default moviesReducer;
