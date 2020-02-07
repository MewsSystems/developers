import axios, {AxiosResponse} from "axios";
import thunk from 'redux-thunk';

export function fetchMovies(searchPhrase,page) {
    return function(dispatch) {
        const baseEndPoint:string = "https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&language=en-US";
        axios.get(`${baseEndPoint}&query=${searchPhrase}&page${page}`)
            .then((response:AxiosResponse<any>)=>{
                dispatch({type:"FETCH_MOVIES_OK",payload:response.data.results});
                //console.log(response.data.results);
            })
    }
}
