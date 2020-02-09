import axios, {AxiosResponse} from "axios";
import Movie from "../models/Movie";

export function fetchMovies(page,searchPhrase) {
    if(searchPhrase) {
        return function (dispatch) {
            dispatch({type: "FETCH_MOVIES_STARTED",})
            const baseEndPoint: string = "https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&language=en-US";
            axios.get(`${baseEndPoint}&query=${searchPhrase}&page=${page}`)
                .then((response: AxiosResponse<any>) => {
                    response.data.results = response.data.results.map((movie)=>{
                        return new Movie( movie.id, movie.title, movie.poster_path, movie.overview)
                    })
                    dispatch({type: "FETCH_MOVIES_SUCCEED", payload: {...response.data, searchPhrase: searchPhrase}});
                })
                .catch(error => {
                    dispatch({type: "FETCH_MOVIES_FAILED"})
                })
        }
    }
    else
        return function (dispatch) {
            dispatch({type: "FETCH_MOVIES_FAILED"})
    }
}
