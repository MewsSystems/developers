import React, {ReactEventHandler, useEffect, useState} from 'react'
import {searchMovies} from "@/services";
import {Movie} from "@/types";
import {Input} from "@mui/material";

export const MoviesList = () => {
    const [movies, setMovies] = useState<Movie[]>([])
    const [query, setQuery] = useState<string>('')

    useEffect(() => {
        searchMovies(query).then((response) => {
            setMovies(response.results)
            }
        )
    }, [query]);

    const handleChange = (event) => {
        const {value} = event.target
        setQuery(value)
    }

    return <div>
        <Input onChange={handleChange} />
        {movies.map(
            movie =>
                <div key={movie.id}>
                    <p>{movie.title}</p>
                    <p>{movie.overview}</p>
                </div>
        )}</div>
}
