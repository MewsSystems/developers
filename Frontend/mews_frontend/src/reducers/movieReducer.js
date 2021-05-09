import { GET_MOVIE, MOVIE_ERROR, SEARCH_MOVIE, FETCH_MOVIES, FETCH_MOVIE } from '../actions/types';

const initialState = {

    movies: [],
    movie: [],
    text: '',
    loading: false,


}

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = initialState, action) {
    switch (action.type) {
        case GET_MOVIE:
            return {
                ...state,
                movies: action.payload,
                loading: false
            }
        case SEARCH_MOVIE:
            return {
                ...state,
                text: action.payload,
                loading: false
            }
        case FETCH_MOVIES:
            return {
                ...state,
                movies: action.payload,
                loading: false
            }
        case FETCH_MOVIE:
            return {
                ...state,
                movie: action.payload,
                loading: false
            }

        case MOVIE_ERROR:
            console.error(action.payload);
            return {
                ...state,
                error: action.payload
            }
        default:
            return state
    }
}