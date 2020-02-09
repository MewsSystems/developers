import movie from '../../models/Movie'
const defaul={
    currentPage: null as number,
    maxPage: null as number
}
export default function moviesReducer(state = defaul ,action) {
    switch (action.type) {
        case "FETCH_MOVIES_SUCCEED":{
            return { ...state,
                currentPage: action.payload.page,
                maxPage: action.payload.total_pages,
            }
        }
    }
    return state
}


