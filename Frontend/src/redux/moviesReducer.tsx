import movie from '../models/movie'
const defaul={
    fetching: null as boolean ,
    fetched:null as boolean,
    movies: null as movie[]

}

export default function moviesReducer(state = {...defaul, fetched: false,fetching: false},action) {
    switch (action.type) {
        case "FETCH_MOVIES_SUCCEED":{
            return { ...state,
                fetching: false,
                fetched: true,
                movies: action.payload,
            }
        }
        case "FETCH_MOVIES_STARTED":{
            return {
                ...state,
                fetching: true,
                fetched: false,
            }
        }
        case "FETCH_MOVIES_FAILED":{
            return {
                ...state,
                fetching: false,
                fetched: false,
            }
        }
    }
    return state
    }


