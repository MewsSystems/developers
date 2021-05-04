import axios from "axios";
import {API_KEY} from "../constants";
import {GetMoviesResponse, MovieDetail} from "../types";

const movieDbApiFactory = () => {
    return{
        getMovies: (movieTitle: string, page: number)=>axios
            .get<GetMoviesResponse>(
                `https://api.themoviedb.org/3/search/movie?api_key=${API_KEY}&language=en-US&query=${movieTitle}&page=${page}&include_adult=false`
            ),
        getMovieDetail: (movieId: string )=>axios
            .get<MovieDetail>(
                `https://api.themoviedb.org/3/movie/${movieId}?api_key=${API_KEY}&language=en-US`
            )
    }
}

export const movieDbApi = movieDbApiFactory();
