import { fetcher } from "../../utils/fetcher";

export const SEARCH_MOVIES_LOADING = "SEARCH_MOVIES_LOADING";
export const SEARCH_MOVIES_SUCCESS = "SEARCH_MOVIES_SUCCESS";
export const SEARCH_MOVIES_FAILURE = "SEARCH_MOVIES_FAILURE";
export const GET_MOVIE_DETAILS_LOADING = "GET_MOVIE_DETAILS_LOADING";
export const GET_MOVIE_DETAILS_SUCCESS = "GET_MOVIE_DETAILS_SUCCESS";
export const GET_MOVIE_DETAILS_FAILURE = "GET_MOVIE_DETAILS_FAILURE";
export const SET_SEARCH_TEXT = 'SET_SEARCH_TEXT';
export const ADD_TO_FAVORITES = 'ADD_TO_FAVORITES';
export const REMOVE_FROM_FAVORITES = 'REMOVE_FROM_FAVORITES';

const API_KEY = process.env.REACT_APP_API_KEY;
const BASE_URL = 'https://api.themoviedb.org/3';

export const searchMovies = (query, page) => (dispatch) => {
    dispatch({type: SEARCH_MOVIES_LOADING});
    return fetcher({
        baseURL: BASE_URL,
        endpointTag:`/search/movie?api_key=${API_KEY}&query=${query}&page=${page}`,
        method: "GET",
    }).then((data) => {
        if(data.status >= 400){
            data.response.then((result) => {
                dispatch({
                    type: SEARCH_MOVIES_FAILURE,
                    title: result.title,
                    details: JSON.stringify(result.errors),
                });
            });
        } else {
            dispatch({ 
                type: SEARCH_MOVIES_SUCCESS, 
                data,
                total_pages: data.total_pages,
            });
        }

    }).catch((error) => {
        dispatch({
          type: SEARCH_MOVIES_FAILURE,
          payload: {
            title: "Error",
            details: error.message,
          },
        });
    });
};

export const getMovieDetails = (movieId) => (dispatch) => {
    dispatch({type: GET_MOVIE_DETAILS_LOADING});
    return fetcher({
        baseURL: BASE_URL,
        endpointTag:`/movie/${movieId}?api_key=${API_KEY}`,
        method: "GET",
    }).then((data) => {
        if(data.status >= 400){
            data.response.then((result) => {
                dispatch({
                    type: GET_MOVIE_DETAILS_FAILURE,
                    title: result.title,
                    details: JSON.stringify(result.errors),
                });
            });
        } else {
            dispatch({ type: GET_MOVIE_DETAILS_SUCCESS, data});
        }

    }).catch((error) => {
        dispatch({
          type: GET_MOVIE_DETAILS_FAILURE,
          payload: {
            title: "Error",
            details: error.message,
          },
        });
    });
};

export const setSearchText = (searchText) => ({
    type: SET_SEARCH_TEXT,
    searchText,
});

export const addToFavorites = (movie) => ({
    type: ADD_TO_FAVORITES,
    payload: movie,
});

export const removeFromFavorites = (movie) => ({
    type: REMOVE_FROM_FAVORITES,
    payload: movie,
});