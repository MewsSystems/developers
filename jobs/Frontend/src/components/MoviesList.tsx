import React, {useEffect, useState} from 'react'
import {searchMovies} from "@/services";
import {Movie} from "@/types";
import {Input, List, ListItemButton, Pagination} from "@mui/material";
import {useNavigate} from "react-router-dom";

export const MoviesList = () => {
    const [movies, setMovies] = useState<Movie[]>([])
    const [query, setQuery] = useState<string>('')
    const navigate = useNavigate();

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

    const handleClick = (id: number) => {
        navigate(`/movie/${id}`);
    }

    return <div>
        <Input onChange={handleChange} placeholder={'Search for a movie'} />
        <List>
            {movies.map(
                movie =>
                    <ListItemButton key={movie.id} onClick={() => handleClick(movie.id)}>
                        {movie.title}
                    </ListItemButton>
            )}
        </List>
        <Pagination />
        </div>
}
