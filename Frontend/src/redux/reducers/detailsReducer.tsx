import Details from '../../models/Details'
const defaul={
    fetching: null as boolean ,
    fetched:null as boolean,
    details:null as Details,
}
export default function detailsReducer(state = {...defaul, fetched: false,fetching: false},action) {
    switch (action.type) {
        case "FETCH_DETAILS_SUCCEED":{
            return { ...state,
                fetching: false,
                fetched: true,
                details: action.payload,
            }
        }
        case "FETCH_DETAILS_STARTED":{
            return {
                ...state,
                fetching: true,
                fetched: false,
            }
        }
        case "FETCH_DETAILS_FAILED":{
            return {
                ...state,
                fetching: false,
                fetched: false,
            }
        }
    }
    return state
}


