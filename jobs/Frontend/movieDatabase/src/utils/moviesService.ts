import {MovieDetails, MovieListItem} from "../types/movies";
import {Keychain} from "../../api";

const API_KEY = Keychain.movieApi;

export async function getMoviesByTitle(searchTitle: string): Promise<MovieListItem[]> {
    try {
        const movies = await fetch(`https://api.themoviedb.org/3/search/movie?api_key=${API_KEY}&language=en-US&page=1&include_adult=false&query=${searchTitle}`)
                           .then(response => response.json())
                           .then(data => {return data.results})
        return movies
    } catch (e) {
        throw new Error('Unable to fetch movies list: ' + e)
    }
}

export async function getMovieById(id: number): Promise<MovieDetails> {
    try {
        const movieDetails = await fetch(`https://api.themoviedb.org/3/movie/${id}?api_key=${API_KEY}&language=en-US`)
                           .then(response => response.json())
                           .then(data => {return data})
        return movieDetails
    } catch (e) {
        throw new Error('Unable to fetch movie detail: ' + e);
    }
}