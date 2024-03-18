import React, {useEffect, useState} from 'react'
import {searchMovies} from "@/services";
import {Movie} from "@/types";

export const MoviesList = () => {
    const [movies, setMovies] = useState<Movie[]>([])

    useEffect(() => {
        const query = 'Alice'
        searchMovies(query).then((response) => {
            setMovies(response.results)
            }
        )
    }, []);

    return <div>
        {movies.map(
            movie =>
                <div key={movie.id}>
                    <p>{movie.title}</p>
                    <p>{movie.overview}</p>
                </div>
        )}</div>
}
