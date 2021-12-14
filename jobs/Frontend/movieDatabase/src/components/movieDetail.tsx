import React, {FC} from "react";
import {MovieDetails} from "../types/movies";

interface MovieDetailProps {
    movie: MovieDetails;
}

export const MovieDetail: FC<MovieDetailProps> = ({movie}) => {

    return (<div>
            <h2 id="MovieDetailTitle">{movie.title}</h2>
            <h4 id="MovieDetailTagline">{movie.tagline ? <i>"{movie.tagline}"</i> : null}</h4>
            <p id="MovieDetailOverView">{movie.overview}</p>
            <p id="MovieDetailBudget">{movie.budget ? <span>Budget: {movie.budget}$</span> : null}</p>
            <p id="MovieDetailRuntime">{movie.runtime ? <span>Runtime: {movie.runtime} Minutes</span> : null}</p>
            <p id="MovieDetailReleased">Released: {movie?.status === 'Released' ? <span>&#9989;</span> : <span>&#10060;</span>}</p>
            <img id="MovieDetailPoster" src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}/>
        </div>)
}