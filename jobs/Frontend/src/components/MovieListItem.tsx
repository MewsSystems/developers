import {Box, ListItemButton, Typography} from "@mui/material";
import {getMoviePosterPath} from "@/utils";
import React from "react";
import {useNavigate} from "react-router-dom";
import {Movie} from "@/types";

interface MovieListItemProps {
    movie: Movie
}

const posterOriginalWidth = 200
const posterWidth = posterOriginalWidth / 4
const posterHeight = posterOriginalWidth / 2.66
const paddingListItem = 8

export const MovieListItem = ({movie}: MovieListItemProps) => {
    const navigate = useNavigate()
    const { id, poster_path, title } = movie

    const handleClick = (id: number) => {
        navigate(`/movie/${id}`)
    }

    return <ListItemButton
        onClick={() => handleClick(id)}
        sx={{ p: paddingListItem + 'px' }}
    >
        {poster_path ? (
            <Box
                sx={{ width: { xs: posterWidth }, height: { xs: posterHeight } }}
                component="img"
                src={getMoviePosterPath(posterOriginalWidth, poster_path)}
            />
        ) : (
            <Box
                sx={{
                    width: { xs: posterWidth },
                    height: { xs: posterHeight },
                    backgroundColor: 'grey',
                }}
            />
        )}
        <Typography color="textSecondary" variant="body1">
            {title}
        </Typography>
    </ListItemButton>
}
