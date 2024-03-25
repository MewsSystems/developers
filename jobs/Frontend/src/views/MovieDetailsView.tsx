import React, {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {Movie} from "@/types";
import {getMovieDetails} from "@/services";
import {getMoviePosterPath} from "@/utils";

export const MovieDetailsView = () => {
    const { id } = useParams();
    const [movie, setMovie] = useState<Movie>([])

    useEffect(() => {
        const idFormatted = parseInt(id)
        getMovieDetails(idFormatted).then((response) => {
                setMovie(response)
            }
        )
    }, []);

    return (
        <div>
            <h1>{movie.title}</h1>
            <p>{movie.overview}</p>
            <p>{movie.release_date}</p>
            <img src={getMoviePosterPath(200, movie.poster_path)}></img>
        </div>
    )
}
