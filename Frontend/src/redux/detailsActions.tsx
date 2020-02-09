import axios, {AxiosResponse} from "axios";
import Movie from "../models/Movie";

export function fetchDetails(id) {
    return function (dispatch) {
        dispatch({type: "FETCH_DETAILS_STARTED",})
        const baseEndPoint: string = "https://api.themoviedb.org/3/movie/";
        const key:string = "?api_key=03b8572954325680265531140190fd2a&language=en-US";
        axios.get(`${baseEndPoint}${id}${key}`)
            .then((response: AxiosResponse<any>) => {
                dispatch({type: "FETCH_DETAILS_SUCCEED", payload: {...response.data, }});
            })
            .catch(error => {
                dispatch({type: "FETCH_DETAILS_FAILED"})
            })
    }

}
