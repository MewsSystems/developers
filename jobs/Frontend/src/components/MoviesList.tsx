import React from 'react'
import { Movie} from "@/types";
import {Box, List, ListItemButton} from "@mui/material";
import {useNavigate} from "react-router-dom";
import {getMoviePosterPath} from "@/utils";

interface MoviesListProps {
    movies: Movie[]
}

export const MoviesList = ({movies}: MoviesListProps) => {
    const navigate = useNavigate();

    const handleClick = (id: number) => {
        navigate(`/movie/${id}`);
    }

    return <List>
        {movies.map(
            movie =>
                <ListItemButton key={movie.id} onClick={() => handleClick(movie.id)}>
                    <Box
                        sx={{width: { xs: 50 }, height: { xs: 75 }}}
                        component="img"
                        src={getMoviePosterPath(200, movie.poster_path)}
                    />
                    {movie.title}
                </ListItemButton>
        )}
    </List>
}
