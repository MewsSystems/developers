import {Box, ListItemButton, styled, Typography} from "@mui/material";
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

const StyledListItemButton = styled(ListItemButton)`
    padding: ${paddingListItem}px;
`

const StyledBox = styled(Box)`
    width: ${posterWidth}px;
    height: ${posterHeight}px;
    
    &.default-img {
        background-color: gray;
    }
`

export const MovieListItem = ({movie}: MovieListItemProps) => {
    const navigate = useNavigate()
    const { id, poster_path, title } = movie

    const handleClick = (id: number) => {
        navigate(`/movie/${id}`)
    }

    return <StyledListItemButton
        onClick={() => handleClick(id)}
    >
        {poster_path ?
            <StyledBox component="img" src={getMoviePosterPath(posterOriginalWidth, poster_path)} alt={title} />
         :
            <StyledBox className={'default-img'} />
        }
        <Typography color="textSecondary" variant="body1">
            {title}
        </Typography>
    </StyledListItemButton>
}
