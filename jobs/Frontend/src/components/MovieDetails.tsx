import {Movie} from "@/types";
import React from "react";
import {getMoviePosterPath} from "@/utils";

interface MovieDetailsProps {
    movie: Movie
}

export const  MovieDetails = ({movie}: MovieDetailsProps) => {
    return <div>
            <h1>{movie.title}</h1>
            <p>{movie.overview}</p>
            <p>{movie.release_date}</p>
            <img src={getMoviePosterPath(200, movie.poster_path)}></img>
    </div>
}
