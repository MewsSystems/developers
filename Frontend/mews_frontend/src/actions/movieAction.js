import { GET_MOVIE, MOVIE_ERROR, SEARCH_MOVIE, FETCH_MOVIES, FETCH_MOVIE } from './types';

export const getMovie = () => async dispatch => {

    try {

        const res = await fetch(`https://api.themoviedb.org/3/movie/popular?api_key=${process.env.REACT_APP_API_KEY}&language=en-US&page=1`);
        const data = await res.json();

        dispatch({
            type: GET_MOVIE,
            payload: data.results
        })
    } catch (err) {
        dispatch({
            type: MOVIE_ERROR,
            payload: err.response.data
        })
    }
}

export const searchMovie = (text) => async dispatch => {

    try {

        dispatch({
            type: SEARCH_MOVIE,
            payload: text
        })
    } catch (err) {
        dispatch({
            type: MOVIE_ERROR,
            payload: err.response.data
        })
    }
}
// Fetch movies
export const fetchMovies = (text) => async dispatch => {

    try {

        const res = await fetch(`https://api.themoviedb.org/3/search/movie?api_key=${process.env.REACT_APP_API_KEY}&language=en-US&page=1&include_adult=false&query=${text}`);
        const data = await res.json();

        dispatch({
            type: FETCH_MOVIES,
            payload: data.results
        })
    } catch (err) {
        dispatch({
            type: MOVIE_ERROR,
            payload: err.response.data
        })
    }
}

// Fetch Movie
export const fetchMovie = (id) => async dispatch => {

    try {

        const res = await fetch(`https://api.themoviedb.org/3/movie/${id}?api_key=${process.env.REACT_APP_API_KEY}&language=en-US`);
        const data = await res.json();

        dispatch({
            type: FETCH_MOVIE,
            payload: data
        })
    } catch (err) {
        dispatch({
            type: MOVIE_ERROR,
            payload: err.response.data
        })
    }
}
// Set loading to true

