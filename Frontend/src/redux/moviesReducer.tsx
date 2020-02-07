import movie from '../models/movie'
const defaul={
    fetching: null as boolean,
    fetched:null as boolean,
    movies: null as movie[]

}

export default function moviesReducer(state = defaul,action) {
    switch (action.type) {
        case "FETCH_MOVIES_OK":{
            return { ...state,
                movies: action.payload,
            }
        }
    }
    return state
    }


