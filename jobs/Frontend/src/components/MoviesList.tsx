import React from 'react'
import { Movie} from "@/types";
import {Box, ListItemButton, ListItem, Typography} from "@mui/material";
import {useNavigate} from "react-router-dom";
import {getMoviePosterPath} from "@/utils";
import {FixedSizeList, ListChildComponentProps} from 'react-window';

interface MoviesListProps {
    movies: Movie[]
}

const posterOriginalWidth = 200
const posterWidth = posterOriginalWidth / 4
const posterHeight = posterOriginalWidth / 2.66
const paddingListItem = 8

export const MoviesList = ({movies}: MoviesListProps) => {
    const renderRow = (props: ListChildComponentProps) => {
        const navigate = useNavigate();
        const { index, style } = props;
        const {id, poster_path, title} = movies[index]

        const handleClick = (id: number) => {
            navigate(`/movie/${id}`);
        }

        return (
            <ListItem style={style} key={id} component="div" disablePadding>
                <ListItemButton onClick={() => handleClick(id)} sx={{ p: paddingListItem + 'px'}} >
                    {poster_path ? <Box
                        sx={{width: {xs: posterWidth}, height: {xs: posterHeight}}}
                        component="img"
                        src={getMoviePosterPath(posterOriginalWidth, poster_path)}
                    /> : <Box sx={{width: {xs: posterWidth}, height: {xs: posterHeight}, backgroundColor: 'grey'}}  />}
                    <Typography color="textSecondary" variant="body1">{title}</Typography>
                </ListItemButton>
            </ListItem>
        );
    }

    return <FixedSizeList
        height={400}
        width={'100%'}
        itemSize={posterHeight + paddingListItem * 2}
        itemCount={movies.length}
        overscanCount={5}
    >
        {renderRow}
    </FixedSizeList>
}
